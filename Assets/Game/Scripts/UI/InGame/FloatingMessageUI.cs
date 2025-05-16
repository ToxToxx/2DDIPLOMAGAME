using UnityEngine;
using TMPro;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InGameUI
{
    public class FloatingMessageUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _textComponent;

        [Header("Settings")]
        [SerializeField] private float _lifetime = 2f;
        [SerializeField] private float _typeSpeed = 0.03f;

        private CancellationTokenSource _cts;
        private void Update()
        {
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180f, 0);
            }
        }

        public void Show(string text)
        {
            CancelExisting();

            gameObject.SetActive(true);
            _cts = new CancellationTokenSource();

            _ = AnimateAsync(text, _cts.Token); // запустить без ожидания
        }

        public void Hide()
        {
            CancelExisting();
            gameObject.SetActive(false);
        }

        private async Task AnimateAsync(string text, CancellationToken token)
        {
            _textComponent.text = string.Empty;

            foreach (char c in text)
            {
                if (token.IsCancellationRequested) return;

                _textComponent.text += c;
                await Task.Delay(TimeSpan.FromSeconds(_typeSpeed), token);
            }

            await Task.Delay(TimeSpan.FromSeconds(_lifetime), token);

            Hide();
        }

        private void CancelExisting()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }
    }
}
