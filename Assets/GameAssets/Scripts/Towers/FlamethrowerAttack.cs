using UnityEngine;

public class FlamethrowerAttack : IAttackBehavior
{
    private readonly float _damagePerSecond;

    public FlamethrowerAttack(float damagePerSecond)
    {
        _damagePerSecond = damagePerSecond;
    }

    public void Attack(Transform target)
    {
        if (target.TryGetComponent<EnemyController>(out var enemy))
        {
            enemy.GetHealthComponent().TakeDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}