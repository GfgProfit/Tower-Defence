using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase, ITowerStats
{
    [Header("Laser Tower Settings")]
    [SerializeField] private LaserBeamEffect _laserBeamEffect;
    [SerializeField] private Transform _firePoint;

    [Space]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _fireRate = 60f;
    
    private float _fireCooldown;

    protected override void Update()
    {
        base.Update();

        if (!CanAttack)
        {
            return;
        }

        if (_fireRate <= 0)
        {
            return;
        }

        HandleAttackCycle();
    }

    private void HandleAttackCycle()
    {
        if (_fireCooldown > 0f)
        {
            _fireCooldown -= Time.deltaTime;

            return;
        }

        TryFireAtTarget();
        ResetCooldown();
    }

    private void TryFireAtTarget()
    {
        if (_currentTarget == null)
        {
            return;
        }

        Vector3 direction = GetDirectionToTarget();
        float distanceToTarget = Vector3.Distance(_firePoint.position, _currentTarget.position) + 1;

        if (Physics.Raycast(_firePoint.position, direction, out RaycastHit hit, distanceToTarget))
        {
            Debug.DrawLine(_firePoint.position, hit.point, Color.red, 0.2f);

            if (hit.collider.TryGetComponent(out EnemyController enemy))
            {
                FireLaserAt(hit.point);
                DealDamage(enemy);
            }
        }
    }

    private Vector3 GetDirectionToTarget() => (_currentTarget.position - _firePoint.position).normalized;

    private void FireLaserAt(Vector3 hitPoint) => _laserBeamEffect.FireLaser(_firePoint.position, hitPoint);

    private void DealDamage(EnemyController enemy) => enemy.GetHealthComponent().TakeDamage(_damage);

    private void ResetCooldown() => _fireCooldown = 60f / _fireRate;

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new("Damage", _damage.ToString()),
            new("Firerate", $"{_fireRate}/min"),
            new("Radius", _visionRange.ToString()),
            new("Rotation Speed", _rotationSpeed.ToString())
        };
    }
}