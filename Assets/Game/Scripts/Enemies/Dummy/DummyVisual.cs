using UnityEngine;
using DG.Tweening;

namespace EnemyDummyLogic
{
    public class DummyVisual : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float punchStrength = 0.2f;
        [SerializeField] private float punchDuration = 0.2f;
        [SerializeField] private float flashDuration = 0.1f;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer != null)
                _originalColor = _spriteRenderer.color;
        }

        public void AnimateHit()
        {
            transform.DOPunchScale(Vector3.one * punchStrength, punchDuration, 10, 1f)
                     .SetEase(Ease.OutExpo);

            if (_spriteRenderer != null)
            {
                _spriteRenderer.DOColor(Color.red, flashDuration)
                    .OnComplete(() => _spriteRenderer.DOColor(_originalColor, flashDuration));
            }
        }
    }
}
