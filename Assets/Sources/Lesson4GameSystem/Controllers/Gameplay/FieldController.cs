using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.Lesson4GameSystem.Controllers.Gameplay
{
    public class FieldController : SerializedMonoBehaviour
    {
        public enum FieldSide
        {
            Center,
            Left,
            Right
        }

        public event Action<ObstacleController> ObstacleDespawnRequired;

        [SerializeField]
        private Dictionary<FieldSide, Vector3> _obstaclePositions = new();

        [SerializeField]
        private Dictionary<FieldSide, Vector3> _playerPositions = new();

        public Vector3 GetSpawnPositionForObstacle(FieldSide side) => _obstaclePositions[side];

        public Vector3 GetMovePositionForPlayer(FieldSide side) => _playerPositions[side];

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var obstaclePosition in _obstaclePositions)
            {
                Gizmos.DrawSphere(obstaclePosition.Value, 0.2f);
            }

            Gizmos.color = Color.blue;
            foreach (var playerPosition in _playerPositions)
            {
                Gizmos.DrawSphere(playerPosition.Value, 0.2f);
            }
        }

        private void OnCollisionEnter(Collision obstacle) =>
            ObstacleDespawnRequired?.Invoke(obstacle.gameObject.GetComponent<ObstacleController>());
    }
}