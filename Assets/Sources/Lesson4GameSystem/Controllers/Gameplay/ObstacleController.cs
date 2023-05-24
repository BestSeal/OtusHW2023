using Sources.Lesson4GameSystem.GameSystem;
using UnityEngine;
using Zenject;

namespace Sources.Lesson4GameSystem.Controllers.Gameplay
{
    public class ObstacleController : MonoBehaviour, IGameUpdatedListener
    {
        private Transform _cashedTransform;
        private Vector3 _velocity;

        private void Awake()
        {
            _cashedTransform = transform;
        }

        private void Reset(Vector3 velocity, Vector3 position)
        {
            _velocity = velocity;
            _cashedTransform.position = position;
        }

        public void OnGameUpdated(float deltaTime) => _cashedTransform.position += _velocity * deltaTime;

        public class ObstaclesPool : MonoMemoryPool<Vector3, Vector3, ObstacleController>
        {
            private readonly GameLoopManager _gameLoopManager;

            public ObstaclesPool(GameLoopManager gameLoopManager)
            {
                _gameLoopManager = gameLoopManager;
            }

            protected override void OnCreated(ObstacleController item)
            {
                base.OnCreated(item);

                if (_gameLoopManager != null)
                {
                    _gameLoopManager.AddListener(item);
                }
            }

            protected override void Reinitialize(Vector3 velocity, Vector3 position, ObstacleController item)
            {
                base.Reinitialize(velocity, position, item);

                item.Reset(velocity, position);
            }
        }
    }
}