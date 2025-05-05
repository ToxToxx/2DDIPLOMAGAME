using UnityEngine;
using DG.Tweening;

namespace EnemyDummyLogic
{
    public class DummyVisual : MonoBehaviour
    {
        [Header("Flash Settings")]
        [SerializeField] private float flashDuration = 0.05f;

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
            if (_spriteRenderer != null)
            {
                _spriteRenderer.material.SetFloat("_FlashAmount", 1f);
                _spriteRenderer.DOColor(Color.white, flashDuration)
                    .OnComplete(() =>
                    {
                        _spriteRenderer.DOColor(_originalColor, flashDuration);
                        _spriteRenderer.material.SetFloat("_FlashAmount", 0f);
                    });
            }
        }
    }
}
