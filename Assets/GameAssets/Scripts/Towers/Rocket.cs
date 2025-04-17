using UnityEngine;

public class Rocket : MonoBehaviour
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

    public void RocketReadyToLaunch(bool value) => IsRocketReadyToLaunch = value;
    public Vector3 GetStartPoint() => StartPoint;
    public Quaternion GetStartRotation() => StartRotation;

    public void TryDealDamage(float damage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyController enemy))
            {
                enemy.GetHealthComponent().TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}