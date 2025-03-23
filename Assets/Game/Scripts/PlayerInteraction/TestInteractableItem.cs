using UnityEngine;

namespace PlayerInteraction
{
    public class TestInteractableItem : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Вы взаимодействовали с " + gameObject.name);
        }
    }
}

