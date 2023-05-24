using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Sources.Lesson4GameSystem.Controllers.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class CounterController : MonoBehaviour
    {
        private TMP_Text _counter;
        private float _elapsedTime;

        private void Awake()
        {
            _counter = GetComponent<TMP_Text>();
            _counter.text = "";
            _counter.gameObject.SetActive(false);
        }

        public void StartCountdown(float startValue, float finishValue = 0, Action countdownFinishedCallback = null)
        {
            _counter.text = startValue.ToString("0.00", CultureInfo.InvariantCulture);
            _counter.gameObject.SetActive(true);
            StartCoroutine(Countdown(startValue, finishValue, countdownFinishedCallback));
        }

        private IEnumerator Countdown(float startValue, float finishValue = 0, Action countdownFinishedCallback = null)
        {
            var leftValue = startValue;
            while (leftValue > finishValue)
            {
                leftValue -= Time.deltaTime;
                _counter.text = leftValue.ToString("0.00", CultureInfo.InvariantCulture);
                yield return null;
            }

            _counter.text = finishValue.ToString("0.00", CultureInfo.InvariantCulture);
            countdownFinishedCallback?.Invoke();
            _counter.gameObject.SetActive(false);
        }
    }
}