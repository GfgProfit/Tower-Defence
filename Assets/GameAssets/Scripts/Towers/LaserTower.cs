using UnityEngine;

public class LaserTower : TowerBase
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _damage = 1;

    protected override IAttackBehavior CreateAttackBehavior()
    {
        return new LaserAttack(_firePoint, _damage);
    }
}