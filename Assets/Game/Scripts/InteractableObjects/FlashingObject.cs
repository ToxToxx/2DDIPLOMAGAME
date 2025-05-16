using UnityEngine;
using DG.Tweening;
using PlayerInteractionLogic;
using InGameUI;

namespace InteractableObjects
{
    [RequireComponent(typeof(Collider2D))]
    public class FlashingObject : MonoBehaviour, IInteractable
    {
        [Header("Flash Settings")]
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private float _flashDuration = 0.4f;
        [SerializeField, Range(0f, 1f)] private float _startFlashAmount = 1f;

        [Header("Proximity")]
        [SerializeField] private string _proximityMessage = "Нажмите E, чтобы взаимодействовать";
        [SerializeField] private FloatingMessageUI _floatingMessage;

        [Header("Visual")]
        [Tooltip("Сюда перетащите объект-спрайт, который будете флашить")]
        [SerializeField] private SpriteRenderer _visualRenderer;

        private Material _matInstance;
        private ProximityMessageTrigger _proximityTrigger;

        // шейдерные ID
        private static readonly int FlashAmtID = Shader.PropertyToID("_FlashAmount");
        private static readonly int FlashColID = Shader.PropertyToID("_FlashColor");

        private void Awake()
        {
            if (_visualRenderer == null)
            {
                Debug.LogError("WellInteractable: Не назначен visualRenderer!", this);
                enabled = false;
                return;
            }

            _matInstance = Instantiate(_visualRenderer.sharedMaterial);
            _visualRenderer.material = _matInstance;

            _proximityTrigger = new ProximityMessageTrigger(_floatingMessage, _proximityMessage);
        }

        private void Start()
        {
            _floatingMessage?.Hide();
        }

        private void OnTriggerEnter2D(Collider2D other)
            => _proximityTrigger.HandleEnter(other);

        private void OnTriggerExit2D(Collider2D other)
            => _proximityTrigger.HandleExit(other);

        public void Interact()
        {
            _matInstance.SetColor(FlashColID, _flashColor);
            _matInstance.SetFloat(FlashAmtID, _startFlashAmount);

            _matInstance
                .DOFloat(0f, FlashAmtID, _flashDuration)
                .SetEase(Ease.OutQuad);
        }
    }
}
