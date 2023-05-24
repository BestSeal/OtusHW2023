using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Lesson4GameSystem.GameSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Sources.Lesson4GameSystem.Controllers.UI
{
    public class InputHelpController : SerializedMonoBehaviour, IGameStartedListener, IGameFinishedListener
    {
        private InputProvider _inputProvider;
        private EventSystem _eventSystem;

        [SerializeField]
        private Dictionary<string, Button> controls;

        [Inject]
        public void Construct(InputProvider inputProvider, EventSystem eventSystem)
        {
            _inputProvider = inputProvider;
            _eventSystem = eventSystem;
        }

        private void Awake()
        {
            _inputProvider.MoveActionPerformed += OnMoveActionPerformed;
            _inputProvider.PauseActionPerformed += PauseActionPerformed;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _inputProvider.MoveActionPerformed -= OnMoveActionPerformed;
            _inputProvider.PauseActionPerformed -= PauseActionPerformed;
        }

        private void PauseActionPerformed(string controlName) => SimulateBtnInput(controlName);

        private void OnMoveActionPerformed(int _, string controlName) => SimulateBtnInput(controlName);

        private void SimulateBtnInput(string controlName)
        {
            if (controls.TryGetValue(controlName, out var button))
            {
                var buttonGO = button.gameObject;
                var baseEventData = new PointerEventData(_eventSystem);
                ExecuteEvents.Execute(buttonGO, baseEventData, ExecuteEvents.pointerEnterHandler);
                ExecuteEvents.Execute(buttonGO, baseEventData, ExecuteEvents.submitHandler);
            }
        }

        public void OnGameStarted() => gameObject.SetActive(true);

        public void OnGameFinished() => gameObject.SetActive(false);
    }
}