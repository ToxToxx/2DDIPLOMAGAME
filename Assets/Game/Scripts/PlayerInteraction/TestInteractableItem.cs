using UnityEngine;

namespace PlayerInteractionLogic
{
    public class TestInteractableItem : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Вы взаимодействовали с " + gameObject.name);
        }
    }
}

