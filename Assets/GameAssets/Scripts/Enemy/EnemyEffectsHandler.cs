using System.Collections;
using UnityEngine;

public class EnemyEffectsHandler : MonoBehaviour, IStunnable
{
    private EnemyMovement _movement;
    private Coroutine _slowCoroutine;
    private bool _isStunned;
    private float _stunTimer;

    public bool IsStunned => _isStunned;

    private void Awake() => _movement = GetComponent<EnemyMovement>();

    private void Update()
    {
        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
            {
                _isStunned = false;
            }
        }
    }

    public void StopEffect()
    {
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
        }

        _movement.SetSpeedMultiplier(1f);
    }

    public void Stun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
        }

        _slowCoroutine = StartCoroutine(SlowRoutine(multiplier, duration));
    }

    private IEnumerator SlowRoutine(float multiplier, float duration)
    {
        _movement.SetSpeedMultiplier(multiplier);
        yield return new WaitForSeconds(duration);
        _movement.ResetSpeed();
    }
}