using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sources.Lesson4GameSystem.GameSystem
{
    public sealed class InputProvider : MonoBehaviour, IGamePausedListener, IGameResumedListener, IGameStartedListener,
        IGameFinishedListener
    {
        public event Action<int, string> MoveActionPerformed;
        public event Action<string> PauseActionPerformed;

        private InputActions _inputActions;

        private void Awake()
        {
            _inputActions = new InputActions();
            _inputActions.Default.Move.performed += OnMovePerformed;
            _inputActions.Default.Pause.performed += OnPausePerformed;
        }

        private void OnDestroy()
        {
            _inputActions.Default.Move.performed -= OnMovePerformed;
            _inputActions.Default.Pause.performed -= OnPausePerformed;
        }

        private void OnPausePerformed(InputAction.CallbackContext callbackContext)
        {
            PauseActionPerformed?.Invoke(callbackContext.control.name);
        }

        private void OnMovePerformed(InputAction.CallbackContext callbackContext)
        {
            MoveActionPerformed?.Invoke((int)callbackContext.ReadValue<float>(), callbackContext.control.name);
        }

        public void OnGamePaused()
        {
            _inputActions.Default.Move.Disable();
        }

        public void OnGameResumed()
        {
            _inputActions.Default.Move.Enable();
        }
        
        public void OnGameStarted()
        {
            _inputActions.Default.Move.Enable();
            _inputActions.Default.Pause.Enable();
        }

        public void OnGameFinished()       
        {
            _inputActions.Default.Move.Disable();
            _inputActions.Default.Pause.Disable();
        }
    }
}