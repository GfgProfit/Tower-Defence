using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase, ITowerStats
{
    [Header("Laser Tower Settings")]
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private FlamethrowerUpgradeConfig _upgradeConfig;

    [Space]
    [SerializeField] private float _damagePerSecond = 10f;
    [SerializeField, Range(0f, 100f)] private float _damageFalloffPercent = 30f;
    [SerializeField, Min(1)] private int _maxEnemiesHit = 4;

    private bool _isEffectPlaying;

    protected override void Update()
    {
        base.Update();

        if (CanAttack)
        {
            HandleAttack();
            StartEffect();
        }
        else
        {
            StopEffect();
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        _upgradeConfig.ApplyUpgrade(this);
    }

    public void UpgradeFlamethrowerTower(float damageMult, float rangeMult, float damageFalloffMult, float rotationSpeedMult)
    {
        _damagePerSecond *= damageMult;
        _visionRange *= rangeMult;
        _damageFalloffPercent *= damageFalloffMult;
        _rotationSpeed *= rotationSpeedMult;
    }

    protected override void UpgradeByAutoLevel()
    {
        _damagePerSecond *= 1.04f;
        _visionRange *= 1.01f;
        _damageFalloffPercent *= 0.9f;
        _rotationSpeed *= 1.015f;
    }

    private void HandleAttack()
    {
        if (_currentTarget == null)
            return;

        Vector3 fireDirection = (_currentTarget.transform.position - _towerHead.position).normalized;
        Ray ray = new(_towerHead.position, fireDirection);

        RaycastHit[] hits = Physics.RaycastAll(ray, _visionRange);

        if (hits.Length == 0)
        {
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        float currentDamage = _damagePerSecond * Time.deltaTime;
        float damageFalloff = 1f - (_damageFalloffPercent / 100f);
        int enemiesHit = 0;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out EnemyBase enemy))
            {
                float actualDamage = enemy.HealthComponent.TakeDamage(_damagePerSecond, this);
                TotalDamageDeal += actualDamage;
                AddExpirience(Mathf.RoundToInt(actualDamage));

                currentDamage *= damageFalloff;
                enemiesHit++;

                if (enemiesHit >= _maxEnemiesHit)
                {
                    break;
                }
            }
        }
    }

    private void StartEffect()
    {
        if (!_isEffectPlaying)
        {
            _fireParticles.Play();
            _isEffectPlaying = true;
        }
    }

    private void StopEffect()
    {
        if (_isEffectPlaying)
        {
            _fireParticles.Stop();
            _isEffectPlaying = false;
        }
    }

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new(_damagePerSecond.ToString("F2")),
            new(_rotationSpeed.ToString("F2")),
            new(_visionRange.ToString("F2")),
            new(_damageFalloffPercent.ToString("F2")),
            new(_maxEnemiesHit.ToString("F2"))
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = (_damagePerSecond * _upgradeConfig.DamageMultiplier) - _damagePerSecond;
        float futureRadius = (_visionRange * _upgradeConfig.VisionRangeMultiplier) - _visionRange;
        float futureDamageFalloff = (_damageFalloffPercent * _upgradeConfig.DamageFalloffMultiplier) - _damageFalloffPercent;
        float futureRotationSpeed = (_rotationSpeed * _upgradeConfig.RotationSpeedMultiplier) - _rotationSpeed;

        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new($"<b><size=20>{upgradeColor}+ {futureDamage:F2}</b></size></color>\n{_damagePerSecond}"),
            new($"<b><size=20>{upgradeColor}+ {futureRadius:F2}</b></size></color>\n{_visionRange}"),
            new($"<b><size=20>{upgradeColor}+ {futureDamageFalloff:F2}</b></size></color>\n{_damageFalloffPercent}"),
            new($"<b><size=20>{upgradeColor}+ {futureRotationSpeed:F2}</b></size></color>\n{_rotationSpeed}"),
            new(_maxEnemiesHit.ToString("F2"))
        };
    }
}
