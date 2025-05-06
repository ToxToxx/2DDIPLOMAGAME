using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    /// <summary>
    ///  Анимация логотипа / кнопок главного меню (Hollow‑Knight‑style):
    ///  • Fade‑in + scale‑in при запуске.
    ///  • Бесконечное «дыхание» (bobbing).
    ///  • HighlightPulse() — импульс масштаба + вспышка цвета без накопления масштаба.
    /// </summary>
    public class MainScreenAnimation : MonoBehaviour
    {
        [Header("Fade & Scale‑In")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 1.2f;
        [SerializeField] private float _scaleDuration = 0.9f;
        [SerializeField] private Ease _fadeEase = Ease.InOutSine;
        [SerializeField] private Ease _scaleEase = Ease.OutBack;

        [Header("Idle Bobbing (breathing)")]
        [SerializeField] private float _bobbingAmplitude = 8f;
        [SerializeField] private float _bobbingPeriod = 3f;
        [SerializeField] private Ease _bobbingEase = Ease.InOutSine;

        [Header("Highlight Pulse")]
        [SerializeField] private float _pulseScale = 1.08f;
        [SerializeField] private float _pulseDuration = 0.25f;
        [SerializeField] private float _pulseFadeBoost = 0.15f;
        [SerializeField] private Color _pulseColor = new Color(1f, 1f, 1f, 0.5f);
        [SerializeField] private Graphic[] _graphicsToFlash;

        private Vector3 _startPos;
        private Vector3 _startScale;
        private Tween _bobbingTween;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            _startPos = transform.localPosition;
            _startScale = transform.localScale;
        }

        private void Start() => PlayIntro();

        /// <summary>
        ///  Вызвать из EventTrigger PointerEnter (или из кода) для вспышки/импульса.
        /// </summary>
        public void HighlightPulse()
        {
            // Scale‑impulse (вверх → обратно)
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(_startScale * _pulseScale, _pulseDuration * 0.5f));
            seq.Append(transform.DOScale(_startScale, _pulseDuration * 0.5f));

            // Лёгкий буст альфы CanvasGroup
            _canvasGroup.DOFade(Mathf.Clamp01(_canvasGroup.alpha + _pulseFadeBoost), _pulseDuration)
                        .SetLoops(2, LoopType.Yoyo);

            // Белый флэш тексту / иконкам
            foreach (var g in _graphicsToFlash)
            {
                if (g == null) continue;
                Color orig = g.color;
                g.DOColor(_pulseColor, _pulseDuration * 0.5f)
                 .SetLoops(2, LoopType.Yoyo)
                 .OnComplete(() => g.color = orig);
            }
        }

        #region Intro & Idle
        private void PlayIntro()
        {
            _canvasGroup.alpha = 0f;
            transform.localScale = _startScale * 0.5f;

            _canvasGroup.DOFade(1f, _fadeDuration).SetEase(_fadeEase);
            transform.DOScale(_startScale, _scaleDuration).SetEase(_scaleEase)
                     .OnComplete(StartIdleBobbing);
        }

        private void StartIdleBobbing()
        {
            _bobbingTween = transform.DOLocalMoveY(_startPos.y + _bobbingAmplitude, _bobbingPeriod / 2f)
                                     .SetEase(_bobbingEase)
                                     .SetLoops(-1, LoopType.Yoyo);
        }
        #endregion

        private void OnDestroy() => _bobbingTween?.Kill();
    }
}
