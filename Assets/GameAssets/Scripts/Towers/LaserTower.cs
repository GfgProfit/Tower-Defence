using UnityEngine;

public class LaserTower : TowerBase
{
    [Header("Laser Tower Settings")]
    [SerializeField] private LaserBeamEffect _laserBeamEffect;
    [SerializeField] private Transform _firePoint;

    [Space]
    [SerializeField] private int _damage = 1;

    protected override IAttackBehavior CreateAttackBehavior()
    {
        return new LaserAttack(_firePoint, _damage);
    }

    protected override void ApplyEffect()
    {
        base.ApplyEffect();

        _laserBeamEffect.FireLaser(_firePoint.position, _currentTarget.position);
    }
}