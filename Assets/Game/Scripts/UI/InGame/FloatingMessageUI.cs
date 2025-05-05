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

        private Coroutine _typingRoutine;

        public void Show(string text)
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);

            gameObject.SetActive(true);
            _typingRoutine = StartCoroutine(TypeText(text));
            Invoke(nameof(Hide), _lifetime);
        }

        private IEnumerator TypeText(string text)
        {
            _textComponent.text = string.Empty;

            foreach (char c in text)
            {
                _textComponent.text += c;
                yield return new WaitForSeconds(_typeSpeed);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180f, 0);
            }
        }
    }
}
