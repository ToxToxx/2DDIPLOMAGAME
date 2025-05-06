using UnityEngine;
using System.Collections.Generic;

namespace Background
{
    public class ParallaxBackground : MonoBehaviour
    {
        [Header("Camera Reference")]
        [SerializeField] private Transform _camera;

        [Header("Layers")]
        [SerializeField] private List<ParallaxLayer> _layers = new();

        [Header("Smoothing & Lookahead")]
        [SerializeField] private float _smoothing = 0.1f;   // 0 → без сглаживания
        [SerializeField] private float _lookaheadFactor = 0.15f;

        private Vector3 _lastCameraPos;      // позиция камеры на предыдущем кадре
        private Vector3 _cameraVelocity;     // сглаженное дельта‑движение
        private Vector3 _initialCameraPos;   // где была камера при старте

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main.transform;

            _initialCameraPos = _camera.position;
            _lastCameraPos = _initialCameraPos;

            // запоминаем, где ты разместил каждый слой
            foreach (var layer in _layers)
                if (layer.LayerTransform != null)
                    layer.InitialPosition = layer.LayerTransform.position;
        }

        private void LateUpdate()
        {
            // движение камеры за кадр
            Vector3 frameDelta = _camera.position - _lastCameraPos;

            // сглаживаем резкие рывки
            _cameraVelocity = Vector3.Lerp(_cameraVelocity, frameDelta, _smoothing);

            // предсказанная позиция камеры (look‑ahead)
            Vector3 predictedDelta = (_camera.position - _initialCameraPos) + (_cameraVelocity * _lookaheadFactor);

            foreach (var layer in _layers)
            {
                if (layer.LayerTransform == null) continue;

                Vector3 offset = new Vector3(
                    predictedDelta.x * layer.ParallaxMultiplier.x,
                    layer.LockY ? 0f : predictedDelta.y * layer.ParallaxMultiplier.y,
                    0f);

                Vector3 target = layer.InitialPosition + offset;

                layer.LayerTransform.position = target;
            }

            _lastCameraPos = _camera.position;
        }
    }
}
