using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : MonoBehaviour, IRocket
{
    [SerializeField] private float _damageRadius = 1.0f;

    public bool IsRocketReadyToLaunch { get; private set; }
    public Vector3 StartPoint { get; private set; }
    public Quaternion StartRotation { get; private set; }
    private Action<float> _onDamageDealt;
    private Action<float> _onExpAdd;
    private TowerBase _owner;

    private void Awake()
    {
        StartPoint = transform.localPosition;
        StartRotation = transform.localRotation;
    }

    public void SetOwner(TowerBase owner)
    {
        _owner = owner;
    }

    public void PrepareForLaunch(float appearanceDuration)
    {
        RocketReadyToLaunch(false);
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, appearanceDuration - 0.1f).SetEase(Ease.InSine);
    }

    public void Launch(Vector3 targetPosition, float flyDuration, float damage, ParticleSystem explosionPrefab)
    {
        RocketReadyToLaunch(true);

        transform
            .DOMove(targetPosition, flyDuration)
            .SetEase(Ease.InQuart)
            .OnComplete(() =>
            {
                TryDealDamage(damage);
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                ResetRocket();
            })
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        transform.LookAt(targetPosition);
    }

    private void ResetRocket()
    {
        transform.SetLocalPositionAndRotation(StartPoint, StartRotation);
        RocketReadyToLaunch(false);
        gameObject.SetActive(false);
    }

    private void RocketReadyToLaunch(bool value) => IsRocketReadyToLaunch = value;

    public void TryDealDamage(float damage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius);
        float totalActualDamage = 0f;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyBase enemy))
            {
                float actualDamage = enemy.HealthComponent.TakeDamage(damage, _owner);
                totalActualDamage += actualDamage;

                _onExpAdd?.Invoke(actualDamage);
            }
        }

        if (totalActualDamage > 0)
        {
            _onDamageDealt?.Invoke(totalActualDamage);
        }
    }

    public void SetDamageCallback(Action<float> onDamageDealt)
    {
        _onDamageDealt = onDamageDealt;
    }

    public void SeeAddExpirienceCallback(Action<float> onAddExp)
    {
        _onExpAdd = onAddExp;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}