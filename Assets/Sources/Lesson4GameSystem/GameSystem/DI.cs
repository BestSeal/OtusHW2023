using Sources.Lesson4GameSystem.Controllers;
using Sources.Lesson4GameSystem.Controllers.Gameplay;
using Sources.Lesson4GameSystem.Controllers.UI;
using Sources.Lesson4GameSystem.Spawners;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Sources.Lesson4GameSystem.GameSystem
{
    public sealed class DI : MonoInstaller
    {
        [SerializeField]
        private GameObject obstaclePrefab;

        [SerializeField]
        private Transform spawnerTransform;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameLoopManager>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<InputProvider>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<FieldController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<ObstaclesSpawner>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<CounterController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<EventSystem>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<StartGameButtonController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<InputHelpController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindMemoryPool<ObstacleController, ObstacleController.ObstaclesPool>()
                .WithInitialSize(9)
                .FromComponentInNewPrefab(obstaclePrefab)
                .UnderTransform(spawnerTransform);
        }
    }
}

