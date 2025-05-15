// Assets/Game/Scripts/Interactables/TextInteractable.cs
using UnityEngine;
using InGameUI;
using PlayerInteractionLogic;
using Player; // PlayerMarker

namespace InteractableObjects
{
    [RequireComponent(typeof(Collider2D))]
    public class TextInteractable : MonoBehaviour, IInteractable
    {
        [Header("Message Settings")]
        [SerializeField] private string[] _messageTexts;
        [SerializeField] private string _proximityMessage = "Нажмите E, чтобы взаимодействовать";
        [SerializeField] private FloatingMessageUI _floatingMessage;

        private int _currentIndex;
        private ProximityMessageTrigger _proximityTrigger;

        private void Awake()
        {
            _proximityTrigger = new ProximityMessageTrigger(_floatingMessage, _proximityMessage);
        }

        private void Start()
        {
            _floatingMessage?.Hide();
        }

        public void Interact()
        {
            if (_messageTexts == null || _messageTexts.Length == 0) return;

            var msg = _messageTexts[_currentIndex];
            _floatingMessage.Show(msg);

            _currentIndex = (_currentIndex + 1) % _messageTexts.Length;
        }

        private void OnTriggerEnter2D(Collider2D other)
            => _proximityTrigger.HandleEnter(other);

        private void OnTriggerExit2D(Collider2D other)
            => _proximityTrigger.HandleExit(other);
    }
}
