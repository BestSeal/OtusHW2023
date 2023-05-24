using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Lesson4GameSystem.Controllers.Gameplay;
using Sources.Lesson4GameSystem.Controllers.UI;
using UnityEngine;
using Zenject;

namespace Sources.Lesson4GameSystem.GameSystem
{
    public sealed class GameLoopManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private List<IGameListener> _listeners = new (20);
        
        [ShowInInspector, ReadOnly]
        private GameLoopState gameLoopState = GameLoopState.Initialization;

        [SerializeField]
        private float gameStartDelay = 3;

        private InputProvider _inputProvider;
        private PlayerController _playerController;
        private CounterController _counterController;
        private StartGameButtonController _startGameButtonController;

        [Inject]
        public void Construct(InputProvider inputProvider, List<IGameListener> gameListeners, 
            PlayerController playerController, CounterController counterController,
                StartGameButtonController startGameButtonController)
        {
            _inputProvider = inputProvider;
            _playerController = playerController;
            _listeners.AddRange(gameListeners);
            _counterController = counterController;
            _startGameButtonController = startGameButtonController;
        }

        public void Awake()
        {
            _inputProvider.PauseActionPerformed += OnPauseActionPerformed;
            _playerController.PlayerCollided += OnPlayerCollided;
            _startGameButtonController.StartGameButtonClicked += OnStartGameButtonClicked;

        }

        private void OnDestroy()
        {
            _inputProvider.PauseActionPerformed -= OnPauseActionPerformed;
            _playerController.PlayerCollided -= OnPlayerCollided;
            _startGameButtonController.StartGameButtonClicked -= OnStartGameButtonClicked;
        }

        private void OnPlayerCollided() => FinishGame();

        private void OnStartGameButtonClicked() => StartGame();

        private void OnPauseActionPerformed(string _)
        {
            switch (gameLoopState)
            {
                case GameLoopState.Playing:
                    PauseGame();
                    break;
                case GameLoopState.Paused:
                    ResumeGame();
                    break;
            }
        }

        public void AddListener(IGameListener listener)
        {
            if (listener == null) return;

            _listeners.Add(listener);
        }

        private void Update()
        {
            if(gameLoopState != GameLoopState.Playing) return;
            
            var deltaTime = Time.deltaTime;
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                if (_listeners[i] is IGameUpdatedListener gameUpdatedListener)
                {
                    gameUpdatedListener.OnGameUpdated(deltaTime);
                }

                count = _listeners.Count;
            }
        }

        [Button]
        public void StartGame()
        {
            if (gameStartDelay > 0 && gameLoopState == GameLoopState.Initialization)
            {
                _counterController.StartCountdown(gameStartDelay, countdownFinishedCallback: StartGameInternal);
            }
            else
            {
                StartGameInternal();
            }
        }
        
        private void StartGameInternal()
        {
            if(gameLoopState != GameLoopState.Initialization) return;

            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                if (_listeners[i] is IGameStartedListener gameStartedListener)
                {
                    gameStartedListener.OnGameStarted();
                }

                count = _listeners.Count;
            }
            
            gameLoopState = GameLoopState.Playing;
        }

        [Button]
        private void FinishGame()
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                if (_listeners[i] is IGameFinishedListener gameFinishedListener)
                {
                    gameFinishedListener.OnGameFinished();
                }

                count = _listeners.Count;
            }
            
            gameLoopState = GameLoopState.Finished;
            
            Debug.LogError("GAME OVER");
        }

        [Button]
        private void PauseGame()
        {
            if(gameLoopState == GameLoopState.Paused) return;
            
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                if (_listeners[i] is IGamePausedListener gamePausedListener)
                {
                    gamePausedListener.OnGamePaused();
                }

                count = _listeners.Count;
            }
            
            gameLoopState = GameLoopState.Paused;
        }

        [Button]
        private void ResumeGame()
        {
            if(gameLoopState == GameLoopState.Playing) return;
            
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                if (_listeners[i] is IGameResumedListener gameResumedListener)
                {
                    gameResumedListener.OnGameResumed();
                }

                count = _listeners.Count;
            }
            
            gameLoopState = GameLoopState.Playing;
        }
    }
}