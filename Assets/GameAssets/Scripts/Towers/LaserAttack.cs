using UnityEngine;

public class LaserAttack : IAttackBehavior
{
    private readonly Transform _firePoint;
    private readonly float _damage;

    public LaserAttack(Transform firePoint, float damage)
    {
        _firePoint = firePoint;
        _damage = damage;
    }

    public void Attack(Transform target)
    {
        Vector3 direction = (target.position - _firePoint.position).normalized;

        if (Physics.Raycast(_firePoint.position, direction, out RaycastHit hit, 100f))
        {
            Debug.DrawLine(_firePoint.position, hit.point, Color.red, 0.2f);
            if (hit.collider.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.GetHealthComponent().TakeDamage(_damage);
            }
        }
    }
}