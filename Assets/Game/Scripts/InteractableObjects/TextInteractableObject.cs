using UnityEngine;
using InGameUI;
using PlayerInteractionLogic;
using Player;

namespace InteractalbeOjbects
{
    [RequireComponent(typeof(Collider2D))]
    public class TextInteractable : MonoBehaviour, IInteractable
    {
        [Header("Message Settings")]
        [SerializeField] private string[] _messageTexts;
        [SerializeField] private string _proximityMessage = "Нажмите E, чтобы взаимодействовать";
        [SerializeField] private FloatingMessageUI _floatingMessage;

        private int _currentMessageIndex = 0;
        private bool _hasShownProximityMessage = false;

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasShownProximityMessage)
                return;

            if (other.TryGetComponent<PlayerMarker>(out _))
            {
                _hasShownProximityMessage = true;

                if (_floatingMessage != null && !string.IsNullOrWhiteSpace(_proximityMessage))
                {
                    _floatingMessage.Show(_proximityMessage);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerMarker>(out _))
            {
                _hasShownProximityMessage = false;
            }
        }

    }
}
