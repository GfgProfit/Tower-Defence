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

    private void HandleAttack()
    {
        if (_currentTarget == null)
            return;

        Vector3 fireDirection = (_currentTarget.transform.position - _towerHead.position).normalized;
        Ray ray = new Ray(_towerHead.position, fireDirection);

        RaycastHit[] hits = Physics.RaycastAll(ray, _visionRange);

        if (hits.Length == 0)
        {
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        float currentDamage = _damagePerSecond * Time.deltaTime;
        float damageFalloff = 1f - (_damageFalloffPercent / 100f); // Перевод процента в коэффициент
        int enemiesHit = 0;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out EnemyController enemy))
            {
                enemy.HealthComponent.TakeDamage(currentDamage);

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
            new("Damage Per Second\n", _damagePerSecond.ToString("F2")),
            new("Rotation Speed\n", _rotationSpeed.ToString("F2")),
            new("Radius\n", _visionRange.ToString("F2")),
            new("Damage Falloff (%)\n", _damageFalloffPercent.ToString("F2")),
            new("Max Enemies Hit\n", _maxEnemiesHit.ToString("F2"))
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = _damagePerSecond * _upgradeConfig.DamageMultiplier;
        float futureRadius = _visionRange * _upgradeConfig.VisionRangeMultiplier;
        float futureDamageFalloff = _damageFalloffPercent * _upgradeConfig.DamageFalloffMultiplier;
        float futureRotationSpeed = _rotationSpeed * _upgradeConfig.RotationSpeedMultiplier;

        string separator = "<color=#FFFFFF>>>></color>";
        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new("Damage Per Second\n", $"{_damagePerSecond:F2} {separator} {upgradeColor}{futureDamage:F2}</color>"),
            new("Rotation Speed\n", $"{_rotationSpeed:F2} {separator} {upgradeColor}{futureRotationSpeed:F2}</color>"),
            new("Radius\n", $"{_visionRange:F2} {separator} {upgradeColor}{futureRadius:F2}</color>"),
            new("Damage Falloff (%)\n", $"{_damageFalloffPercent:F2} {separator} {upgradeColor}{futureDamageFalloff:F2}</color>"),
            new("Max Enemies Hit\n", _maxEnemiesHit.ToString("F2"))
        };
    }
}
