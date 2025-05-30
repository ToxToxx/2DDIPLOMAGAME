using UnityEngine;
using TMPro;
using System.Collections;

namespace InGameUI
{
    public class FloatingMessageUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _textComponent;

        [Header("Settings")]
        [SerializeField] private float _lifetime = 2f;
        [SerializeField] private float _typeSpeed = 0.03f;

        private Coroutine _animateCoroutine;

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
            _animateCoroutine = StartCoroutine(AnimateCoroutine(text));
        }

        public void Hide()
        {
            CancelExisting();
            gameObject.SetActive(false);
        }

        private IEnumerator AnimateCoroutine(string text)
        {
            _textComponent.text = string.Empty;

            foreach (char c in text)
            {
                _textComponent.text += c;
                yield return new WaitForSeconds(_typeSpeed);
            }

            yield return new WaitForSeconds(_lifetime);

            Hide();
        }

        private void CancelExisting()
        {
            if (_animateCoroutine != null)
            {
                StopCoroutine(_animateCoroutine);
                _animateCoroutine = null;
            }
        }
    }
}
