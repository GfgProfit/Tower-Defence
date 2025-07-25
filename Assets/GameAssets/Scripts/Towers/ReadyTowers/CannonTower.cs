using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CannonTower : TowerBase, ITowerStats
{
    [Header("Cannon Tower Settings")]
    [SerializeField] private ParticleSystem _shotParticles;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _cannonPointTransform;
    [SerializeField] private CannonUpgradeConfig _upgradeConfig;

    [Space]
    [SerializeField] private float _damage = 1;
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

        if (_currentTarget == null)
        {
            return;
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        _upgradeConfig.ApplyUpgrade(this);
    }

    protected override void UpgradeByAutoLevel()
    {
        _damage *= 1.05f;
        _fireRate *= 1.08f;
        _visionRange *= 1.01f;
        _rotationSpeed *= 1.01f;
    }

    public void UpgradeCannonTower(float damageMult, float fireRateMult, float rangeMult, float rotationSpeedMult)
    {
        _damage *= damageMult;
        _fireRate *= fireRateMult;
        _visionRange *= rangeMult;
        _rotationSpeed *= rotationSpeedMult;
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

            if (hit.collider.TryGetComponent(out EnemyBase enemy))
            {
                if (enemy.HealthComponent.IsDead)
                {
                    _currentTarget = null;
                    return;
                }

                PlayShotParticles();
                DealDamage(enemy);

                DOTween.Sequence()
                    .Append(_cannonPointTransform.DOLocalMoveZ(-0.2f, 0.2f))
                    .Append(_cannonPointTransform.DOLocalMoveZ(0, 0.2f))
                    .SetEase(Ease.OutQuart);
            }
        }
    }

    private Vector3 GetDirectionToTarget() => (_currentTarget.position - _firePoint.position).normalized;

    private void PlayShotParticles() => _shotParticles.Play();

    private void DealDamage(EnemyBase enemy)
    {
        float actualDamage = enemy.HealthComponent.TakeDamage(_damage, this);
        TotalDamageDeal += actualDamage;
        _sceneTotalHandler.TotalDamageDeal += actualDamage;
        AddExpirience(actualDamage);
    }

    private void ResetCooldown() => _fireCooldown = 60f / _fireRate;

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new("Damage", _damage.ToString("F2")),
            new("Fire Rate/min", _fireRate.ToString("F2")),
            new("Radius", _visionRange.ToString("F2")),
            new("Rotation Speed", _rotationSpeed.ToString("F2"))
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = (_damage * _upgradeConfig.DamageMultiplier) - _damage;
        float futureFireRate = (_fireRate * _upgradeConfig.FireRateMultiplier) - _fireRate;
        float futureRadius = (_visionRange * _upgradeConfig.VisionRangeMultiplier) - _visionRange;
        float futureRotationSpeed = (_rotationSpeed * _upgradeConfig.RotationSpeedMultiplier) - _rotationSpeed;

        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new("Damage", $"{_damage:F2}<b><size=20>{upgradeColor} +{futureDamage:F2}</b></size></color>"),
            new("Fire Rate/min", $"{_fireRate:F2}<b><size=20>{upgradeColor} +{futureFireRate:F2}</b></size></color>"),
            new("Radius", $"{_visionRange:F2}<b><size=20>{upgradeColor} +{futureRadius:F2}</b></size></color>"),
            new("Rotation Speed", $"{_rotationSpeed:F2}<b><size=20>{upgradeColor} +{futureRotationSpeed:F2}</b></size></color>"),
        };
    }
}