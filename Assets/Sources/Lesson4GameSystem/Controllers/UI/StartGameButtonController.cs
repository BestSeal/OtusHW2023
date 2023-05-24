using System;
using Sources.Lesson4GameSystem.GameSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Lesson4GameSystem.Controllers.UI
{
    [RequireComponent(typeof(Button))]
    public class StartGameButtonController : MonoBehaviour, IGameStartedListener, IPointerClickHandler
    {
        public event Action StartGameButtonClicked;

        public void OnGameStarted() => gameObject.SetActive(false);

        public void OnPointerClick(PointerEventData eventData)
        {
            StartGameButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }
    }
}