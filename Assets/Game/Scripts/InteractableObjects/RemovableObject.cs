using UnityEngine;
using DG.Tweening;
using PlayerInteractionLogic;
using InGameUI;
using Player;

namespace InteractableObjects
{
    [RequireComponent(typeof(Collider2D))]
    public class RemovableObject : MonoBehaviour, IInteractable
    {
        [Header("Proximity")]
        [SerializeField] private string _proximityMessage = "Нажмите E, чтобы убрать объект";
        [SerializeField] private FloatingMessageUI _floatingMessage;

        [Header("Target Collider")]
        [Tooltip("Коллайдер дочернего объекта, который отключится при взаимодействии")]
        [SerializeField] private Collider2D _targetCollider;

        [Header("Animation Settings")]
        [SerializeField] private Transform _animatedTransform;
        [SerializeField] private float _moveOffsetY = -1f;
        [SerializeField] private float _rotationZ = 90f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        private ProximityMessageTrigger _proximityTrigger;
        private Collider2D _triggerCollider;
        private bool _hasRemoved;

        private void Awake()
        {
            _triggerCollider = GetComponent<Collider2D>();
            _proximityTrigger = new ProximityMessageTrigger(_floatingMessage, _proximityMessage);

            if (_animatedTransform == null)
                _animatedTransform = transform;
        }

        private void Start()
        {
            _floatingMessage?.Hide();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasRemoved) return;

            _proximityTrigger.HandleEnter(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_hasRemoved) return;
            _proximityTrigger.HandleExit(other);
        }

        public void Interact()
        {
            if (_hasRemoved) return;
            _hasRemoved = true;

            if (_targetCollider != null)
                _targetCollider.enabled = false;

            _floatingMessage?.Hide();

            var t = _animatedTransform;
            float toY = t.localPosition.y + _moveOffsetY;
            float toZ = _rotationZ;

            DOTween.Sequence()
                .Append(t.DOLocalMoveY(toY, _duration).SetEase(_ease))
                .Join(t.DOLocalRotate(new Vector3(0, 0, toZ), _duration, RotateMode.LocalAxisAdd).SetEase(_ease))
                .Play();
        }


    }
}
