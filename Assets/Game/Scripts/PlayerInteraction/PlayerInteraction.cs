using UnityEngine;

namespace PlayerInteractionLogic
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionRadius = 1.5f; 
        [SerializeField] private LayerMask _interactableLayer;  

        private void Update()
        {
            if (InputManager.InteractionWasPressed)
            {
                Interact();
            }
        }

        private void Interact()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactionRadius, _interactableLayer);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Interact(); 
                }
            }
        }
    }
}