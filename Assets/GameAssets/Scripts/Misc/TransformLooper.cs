using UnityEngine;
using DG.Tweening;

public class TransformLooper : MonoBehaviour
{
    [SerializeField] private Transform[] _upwardTransforms;
    [SerializeField] private Transform[] _downwardTransforms;

    [Space]
    [SerializeField] private float _moveDistance = 2f;
    [SerializeField] private float _moveDuration = 1f;

    private Tween[] _tweens;

    private void Start()
    {
        StartLoop();
    }

    private void StartLoop()
    {
        int count = _upwardTransforms.Length + _downwardTransforms.Length;
        _tweens = new Tween[count];
        int index = 0;

        foreach (Transform transform in _upwardTransforms)
        {
            Vector3 startPos = transform.position;
            _tweens[index++] = transform.DOMoveY(startPos.y + _moveDistance, _moveDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(transform.gameObject, LinkBehaviour.KillOnDestroy);
        }

        foreach (Transform transform in _downwardTransforms)
        {
            Vector3 startPos = transform.position;
            _tweens[index++] = transform.DOMoveY(startPos.y - _moveDistance, _moveDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(transform.gameObject, LinkBehaviour.KillOnDestroy);
        }
    }

    private void OnDestroy()
    {
        if (_tweens == null)
        {
            return;
        }

        foreach (Tween tween in _tweens)
        {
            tween?.Kill();
        }
    }
}