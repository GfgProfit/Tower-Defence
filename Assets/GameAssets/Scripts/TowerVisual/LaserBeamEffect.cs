using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeamEffect : MonoBehaviour
{
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _fadeDuration = 0.1f;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    public void FireLaser(Vector3 startPoint, Vector3 endPoint)
    {
        _lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);
        _lineRenderer.enabled = true;

        Color startColor = new(0, 208.0f / 255.0f, 231.0f / 255.0f)
        {
            a = 1f
        };

        _lineRenderer.startColor = startColor;
        _lineRenderer.endColor = startColor;

        DOVirtual.DelayedCall(_duration, () =>
        {
            DOTween.ToAlpha(() => _lineRenderer.startColor, c => _lineRenderer.startColor = c, 0f, _fadeDuration);
            DOTween.ToAlpha(() => _lineRenderer.endColor, c => _lineRenderer.endColor = c, 0f, _fadeDuration)
                .OnComplete(() => _lineRenderer.enabled = false);
        });
    }
}