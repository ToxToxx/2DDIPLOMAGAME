using InGameInput;
using UnityEngine;
using Zenject;

namespace PlayerInteractionLogic
{
    public class PlayerInteraction : ITickable
    {
        private readonly IInputService _input;
        private readonly Transform _transform;
        private readonly float _interactionRadius;
        private readonly LayerMask _interactableLayer;

        public PlayerInteraction(Transform transform, float interactionRadius, LayerMask interactableLayer, IInputService input)
        {
            _transform = transform;
            _interactionRadius = interactionRadius;
            _interactableLayer = interactableLayer;
            _input = input;
        }

        public void Tick()
        {
            if (_input.InteractionWasPressed) Interact();
        }

        private void Interact()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, _interactionRadius, _interactableLayer);
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