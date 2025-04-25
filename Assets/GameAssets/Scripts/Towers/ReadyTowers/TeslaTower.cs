using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : TowerBase, ITowerStats
{
    [Header("Tesla Tower Settings")]
    [SerializeField] private LineRenderer _lineRendererPrefab;
    [SerializeField] private TeslaUpgradeConfig _upgradeConfig;

    [Space]
    [SerializeField] private float _damage = 1f;
    [SerializeField, Range(0f, 100f)] private float _damageFalloffPercent = 30f;
    [SerializeField] private int _maxChainCount = 3;
    [SerializeField] private float _chainRadius = 5f;
    [SerializeField] private float _stunDuration = 0.1f;
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

        HandleChainLightning();
    }

    public override void Upgrade()
    {
        base.Upgrade();

        _upgradeConfig.ApplyUpgrade(this);
    }

    protected override void UpgradeByAutoLevel()
    {
        base.UpgradeByAutoLevel();

        _damage *= 1.06f;
        _fireRate *= 1.05f;
        _visionRange *= 1.03f;
        _damageFalloffPercent *= 1.02f;
        _stunDuration *= 1.02f;
    }

    public void UpgradeTeslaTower(float damageMult, float fireRateMult, float rangeMult, float damageFalloffMult, float stunDurationMult)
    {
        _damage *= damageMult;
        _fireRate *= fireRateMult;
        _visionRange *= rangeMult;
        _damageFalloffPercent *= damageFalloffMult;
        _stunDuration *= stunDurationMult;
    }

    private void HandleChainLightning()
    {
        if (_fireCooldown > 0f)
        {
            _fireCooldown -= Time.deltaTime;

            return;
        }

        StartCoroutine(ChainLightningCoroutine(_currentTarget));

        ResetCooldown();
    }

    private IEnumerator ChainLightningCoroutine(Transform startTarget)
    {
        List<EnemyBase> hitEnemies = new();
        List<Vector3> points = new();

        float currentDamage = _damage;
        float damageFalloff = 1f - (_damageFalloffPercent / 100f);

        if (startTarget.TryGetComponent(out EnemyBase currentTarget))
        {
            points.Add(_towerHead.position);
            points.Add(currentTarget.transform.position);

            if (points.Count >= 2)
            {
                SpawnLightning(points);
            }

            for (int i = 0; i < _maxChainCount; i++)
            {
                if (currentTarget == null)
                    break;

                DealDamage(currentTarget, currentDamage);
                StunEnemy(currentTarget);

                hitEnemies.Add(currentTarget);

                currentTarget = FindClosestEnemy(currentTarget.transform.position, hitEnemies);

                currentDamage *= damageFalloff;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void SpawnLightning(List<Vector3> points)
    {
        LineRenderer currentLineRenderer = Instantiate(_lineRendererPrefab);
        currentLineRenderer.useWorldSpace = true;

        List<Vector3> distortedPoints = new();

        for (int i = 0; i < points.Count - 1; i++)
        {
            distortedPoints.Add(points[i]);

            Vector3 midPoint = (points[i] + points[i + 1]) / 2f;

            midPoint += (1f + Random.Range(-0.2f, 0.2f)) * 0.5f * Random.insideUnitSphere;

            distortedPoints.Add(midPoint);
        }

        distortedPoints.Add(points[^1]);

        currentLineRenderer.positionCount = distortedPoints.Count;
        currentLineRenderer.SetPositions(distortedPoints.ToArray());

        Destroy(currentLineRenderer.gameObject, 0.2f);
    }

    private void DealDamage(EnemyBase enemy, float damage)
    {
        float actualDamage = enemy.HealthComponent.TakeDamage(damage, this);
        TotalDamageDeal += actualDamage;
        AddExpirience(Mathf.RoundToInt(actualDamage));
    }

    private void ResetCooldown() => _fireCooldown = 60f / _fireRate;

    private void StunEnemy(EnemyBase enemy)
    {
        if (enemy.TryGetComponent(out IStunnable stunnable))
        {
            stunnable.Stun(_stunDuration);
        }
    }

    private EnemyBase FindClosestEnemy(Vector3 fromPosition, List<EnemyBase> excludedEnemies)
    {
        Collider[] colliders = Physics.OverlapSphere(fromPosition, _chainRadius);

        EnemyBase closest = null;
        float minDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyBase enemy))
            {
                if (excludedEnemies.Contains(enemy))
                {
                    continue;
                }

                if (enemy.HealthComponent.IsDead)
                {
                    _currentTarget = null;
                    continue;
                }

                float distance = Vector3.Distance(fromPosition, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new(_damage.ToString("F2")),
            new(_visionRange.ToString("F2")),
            new(_fireRate.ToString("F2")),
            new(_damageFalloffPercent.ToString("F2")),
            new(_stunDuration.ToString("F2")),
            new(_rotationSpeed.ToString("F2")),
            new(_maxChainCount.ToString("F2"))
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = (_damage * _upgradeConfig.DamageMultiplier) - _damage;
        float futureFireRate = (_fireRate * _upgradeConfig.FireRateMultiplier) - _fireRate;
        float futureRadius = (_visionRange * _upgradeConfig.VisionRangeMultiplier) - _visionRange;
        float futureDamageFalloff = (_damageFalloffPercent * _upgradeConfig.DamageFalloffMultiplier) - _damageFalloffPercent;
        float futureStunDuration = (_stunDuration * _upgradeConfig.StunDurationMultiplier) - _stunDuration;

        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new($"<b><size=20>{upgradeColor}+ {futureDamage:F2}</b></size></color>\n{_damage}"),
            new($"<b><size=20>{upgradeColor}+ {futureFireRate:F2}</b></size></color>\n{_fireRate}"),
            new($"<b><size=20>{upgradeColor}+ {futureRadius:F2}</b></size></color>\n{_visionRange}"),
            new($"<b><size=20>{upgradeColor}+ {futureDamageFalloff:F2}</b></size></color>\n{_damageFalloffPercent}"),
            new($"<b><size=20>{upgradeColor}+ {futureStunDuration:F2}</b></size></color>\n{_stunDuration}"),
            new(_rotationSpeed.ToString("F2")),
            new(_maxChainCount.ToString("F2"))
        };
    }
}