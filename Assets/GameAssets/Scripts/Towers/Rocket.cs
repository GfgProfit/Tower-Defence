using DG.Tweening;
using UnityEngine;

public class Rocket : MonoBehaviour, IRocket
{
    [SerializeField] private float _damageRadius = 1.0f;

    public bool IsRocketReadyToLaunch { get; private set; }
    public Vector3 StartPoint { get; private set; }
    public Quaternion StartRotation { get; private set; }

    private void Awake()
    {
        StartPoint = transform.localPosition;
        StartRotation = transform.localRotation;
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

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyController enemy))
            {
                enemy.HealthComponent.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}