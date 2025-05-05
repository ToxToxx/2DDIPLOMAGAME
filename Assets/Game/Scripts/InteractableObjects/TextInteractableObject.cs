using UnityEngine;
using InGameUI;

namespace PlayerInteractionLogic
{
    public class TextInteractable : MonoBehaviour, IInteractable
    {
        [Header("Message Settings")]
        [SerializeField] private string[] _messageTexts;
        [SerializeField] private FloatingMessageUI _floatingMessage;

        private int _currentMessageIndex = 0;

        private void Start()
        {
            if (_floatingMessage != null)
                _floatingMessage.Hide();
        }

        public void Interact()
        {
            if (_messageTexts == null || _messageTexts.Length == 0)
                return;

            string message = _messageTexts[_currentMessageIndex];

            Debug.Log($"Интеракция с {gameObject.name}: {message}");

            _floatingMessage.Show(message);

            _currentMessageIndex = (_currentMessageIndex + 1) % _messageTexts.Length;
        }
    }
}
