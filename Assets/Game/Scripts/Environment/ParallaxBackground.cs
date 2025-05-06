using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;

    [Header("Parallax Settings")]
    [SerializeField] private Vector2 _parallaxMultiplier = new Vector2(0.8f, 0.8f); // ближе к 1 для стабильности
    [SerializeField] private bool _lockY = false;

    [Header("Smoothing & Lookahead")]
    [SerializeField] private float _smoothing = 0.05f;       // интерполяция (чем ниже, тем плавнее)
    [SerializeField] private float _lookaheadFactor = 0.2f;   // предсказание движения

    private Vector3 _lastCameraPosition;
    private Vector3 _targetPosition;
    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        if (_camera == null)
            _camera = Camera.main?.transform;

        _lastCameraPosition = _camera.position;
        _targetPosition = transform.position;

        StartCoroutine(TrackCameraLate());
    }

    private IEnumerator TrackCameraLate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            Vector3 delta = _camera.position - _lastCameraPosition;
            Vector3 lookahead = delta * _lookaheadFactor;

            Vector3 desiredPos = _camera.position + lookahead;

            _targetPosition = new Vector3(
                desiredPos.x * _parallaxMultiplier.x,
                (_lockY ? transform.position.y : desiredPos.y * _parallaxMultiplier.y),
                transform.position.z
            );

            transform.position = Vector3.SmoothDamp(
                transform.position,
                _targetPosition,
                ref _velocity,
                _smoothing
            );

            _lastCameraPosition = _camera.position;
        }
    }
}
