using System;
using Sources.Lesson4GameSystem.GameSystem;
using UnityEngine;
using Zenject;

namespace Sources.Lesson4GameSystem.Controllers.Gameplay
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerController : MonoBehaviour, IGameStartedListener
    {
        [SerializeField]
        private LayerMask obstaclesLayer;
        public event Action PlayerCollided;
        
        private InputProvider _inputProvider;
        private FieldController _fieldController;
        private Transform _cashedTransform;
        private int _positionIndex;

        [Inject]
        private void Construct(InputProvider inputProvider, FieldController fieldController)
        {
            _inputProvider = inputProvider;
            _fieldController = fieldController;
        }

        private void Awake()
        {
            _inputProvider.MoveActionPerformed += OnMoveActionPerformed;
            _cashedTransform = gameObject.transform;
            _positionIndex = 0;
        }

        private void OnDestroy()
        {
            _inputProvider.MoveActionPerformed -= OnMoveActionPerformed;
        }

        private void OnMoveActionPerformed(int direction, string _ = null)
        {
            _positionIndex = Mathf.Clamp(direction + _positionIndex, -1, 1);
            _cashedTransform.position = _positionIndex switch
            {
                0 => _fieldController.GetMovePositionForPlayer(FieldController.FieldSide.Center),
                1 => _fieldController.GetMovePositionForPlayer(FieldController.FieldSide.Right),
                -1 => _fieldController.GetMovePositionForPlayer(FieldController.FieldSide.Left),
                _ => _cashedTransform.position
            };
        }

        private void OnCollisionEnter(Collision collided)
        {
            if((obstaclesLayer.value & (1 << collided.gameObject.layer)) > 0)
            {
                PlayerCollided?.Invoke();
            }
        }

        public void OnGameStarted() => OnMoveActionPerformed(_positionIndex);
    }
}