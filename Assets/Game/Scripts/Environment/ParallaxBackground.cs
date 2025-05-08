using UnityEngine;
using System.Collections.Generic;

namespace Background
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform LayerTransform;

        [Tooltip("Насколько слой смещается по X относительно движения камеры")]
        [Range(0f, 1f)]
        public float ParallaxMultiplierX = 0.1f;

        [Tooltip("Насколько слой смещается по Y относительно движения камеры")]
        [Range(0f, 1f)]
        public float ParallaxMultiplierY = 0.1f;

        [Tooltip("Отключить вертикальный параллакс для этого слоя")]
        public bool DisableVerticalParallax = false;
    }

    public class ParallaxBackground : MonoBehaviour
    {
        [Header("Camera Reference")]
        [SerializeField] private Transform _camera;

        [Header("Layers")]
        [SerializeField] private List<ParallaxLayer> _layers = new();

        private Vector3 _lastCameraPosition;

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main.transform;

            _lastCameraPosition = _camera.position;
        }

        private void LateUpdate()
        {
            Vector3 delta = _camera.position - _lastCameraPosition;

            foreach (var layer in _layers)
            {
                if (layer.LayerTransform == null)
                    continue;

                Vector3 pos = layer.LayerTransform.position;
                // Horizontal parallax
                pos.x += delta.x * layer.ParallaxMultiplierX;

                // Vertical parallax per layer
                float verticalDelta = layer.DisableVerticalParallax ? 0f : delta.y;
                pos.y += verticalDelta * layer.ParallaxMultiplierY;

                layer.LayerTransform.position = pos;
            }

            _lastCameraPosition = _camera.position;
        }
    }
}
