using Sirenix.OdinInspector;
using Sources.Lesson4GameSystem.Controllers;
using Sources.Lesson4GameSystem.Controllers.Gameplay;
using Sources.Lesson4GameSystem.GameSystem;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Lesson4GameSystem.Spawners
{
    public class ObstaclesSpawner : MonoBehaviour, IGameUpdatedListener
    {
        [SerializeField]
        private float spawnDelay;

        [SerializeField]
        private Vector3 obstaclesVelocity;

        private ObstacleController.ObstaclesPool _obstaclesPool;
        private FieldController _fieldController;
        [ShowInInspector, ReadOnly]
        private float _timeSinceLastSpawn;

        [Inject]
        private void Construct(ObstacleController.ObstaclesPool obstaclesPool, FieldController fieldController)
        {
            _obstaclesPool = obstaclesPool;
            _fieldController = fieldController;
        }

        private void Awake()
        {
            _fieldController.ObstacleDespawnRequired += OnObstacleDespawnRequired;
        }

        private void OnDestroy()
        {
            _fieldController.ObstacleDespawnRequired -= OnObstacleDespawnRequired;
        }

        private void OnObstacleDespawnRequired(ObstacleController obstacle)
        {
            _obstaclesPool.Despawn(obstacle);   
        }

        public void OnGameUpdated(float deltaTime)
        {
            if (_timeSinceLastSpawn >= spawnDelay)
            {
                _timeSinceLastSpawn = 0;
                var side = Random.Range(-1, 2);

                Vector3 position = side switch
                {
                    0 => _fieldController.GetSpawnPositionForObstacle(FieldController.FieldSide.Center),
                    1 => _fieldController.GetSpawnPositionForObstacle(FieldController.FieldSide.Right),
                    -1 => _fieldController.GetSpawnPositionForObstacle(FieldController.FieldSide.Left),
                    _ => Vector3.zero
                };

                _obstaclesPool.Spawn(obstaclesVelocity, position);
            }
            else
            {
                _timeSinceLastSpawn += deltaTime;
            }
        }
    }
}