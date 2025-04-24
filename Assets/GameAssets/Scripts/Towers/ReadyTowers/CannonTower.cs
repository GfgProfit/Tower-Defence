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

            if (hit.collider.TryGetComponent(out EnemyController enemy))
            {
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

    private void DealDamage(EnemyController enemy)
    {
        float actualDamage = enemy.HealthComponent.TakeDamage(_damage, this);
        TotalDamageDeal += actualDamage;
        AddExpirience(Mathf.RoundToInt(actualDamage));
    }

    private void ResetCooldown() => _fireCooldown = 60f / _fireRate;

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new("Damage\n", _damage.ToString("F2")),
            new("Rotation Speed\n", _rotationSpeed.ToString("F2")),
            new("Radius\n", _visionRange.ToString("F2")),
            new("Fire Rate (min)\n", $"{_fireRate:F2}")
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = _damage * _upgradeConfig.DamageMultiplier;
        float futureFireRate = _fireRate * _upgradeConfig.FireRateMultiplier;
        float futureRadius = _visionRange * _upgradeConfig.VisionRangeMultiplier;
        float futureRotationSpeed = _rotationSpeed * _upgradeConfig.RotationSpeedMultiplier;

        string separator = "<color=#FFFFFF>>>></color>";
        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new("Damage\n", $"{_damage:F2} {separator} {upgradeColor}{futureDamage:F2}</color>"),
            new("Rotation Speed\n", $"{_rotationSpeed:F2} {separator} {upgradeColor}{futureRotationSpeed:F2}</color>"),
            new("Radius\n", $"{_visionRange:F2} {separator} {upgradeColor}{futureRadius:F2}</color>"),
            new("Fire Rate (min)\n", $"{_fireRate:F2} {separator} {upgradeColor}{futureFireRate:F2}</color>")
        };
    }
}