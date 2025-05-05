using UnityEngine;
using InGameUI;

namespace PlayerInteractionLogic
{
    public class TextInteractable : MonoBehaviour, IInteractable
    {
        [Header("Message Settings")]
        [SerializeField] private string _messageText = "Это объект для взаимодействия.";
        [SerializeField] private FloatingMessageUI _floatingMessage;

        private void Start()
        {
            _floatingMessage.Hide();
        }
        public void Interact()
        {
            Debug.Log("Вы взаимодействовали с " + gameObject.name);

            _floatingMessage.Show(_messageText);

        }
    }
}
