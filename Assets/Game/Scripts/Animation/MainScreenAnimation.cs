using DG.Tweening;
using UnityEngine;

namespace MainMenu
{
    public class MainScreenAnimation : MonoBehaviour
    {
        [Header("Настройки появления")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration = 1.5f; // Время появления из темноты
        [SerializeField] private float _scaleInDuration = 1.2f; // Время увеличения с размытия
        [SerializeField] private Ease _fadeEase = Ease.InOutSine; // Плавность появления
        [SerializeField] private Ease _scaleEase = Ease.OutExpo; // Легкое увеличение

        [Header("Эффекты движения")]
        [SerializeField] private float _bobbingDistance = 10f; // Движение вверх-вниз
        [SerializeField] private float _bobbingDuration = 2.0f; // Цикл колебания
        [SerializeField] private float _shakeIntensity = 2f; // Сила дрожания
        [SerializeField] private float _shakeDuration = 3.0f; // Интервал дрожания

        private void Start()
        {
            PlayMysticAnimation();
        }

        private void PlayMysticAnimation()
        {
            // Начальное состояние — спрятано и уменьшено
            _canvasGroup.alpha = 0f;
            transform.localScale = Vector3.zero;

            // Появление из темноты и увеличение
            _canvasGroup.DOFade(1f, _fadeInDuration).SetEase(_fadeEase);
            transform.DOScale(1f, _scaleInDuration).SetEase(_scaleEase).OnComplete(() =>
            {
                // Колебание вверх-вниз всей группы
                transform.DOMoveY(transform.position.y + _bobbingDistance, _bobbingDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);

                // Легкое дрожание всей группы
                transform.DOShakeRotation(_shakeDuration, _shakeIntensity);
            });
        }
    }
}
