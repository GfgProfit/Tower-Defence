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
        List<EnemyController> hitEnemies = new();
        List<Vector3> points = new();

        float currentDamage = _damage;
        float damageFalloff = 1f - (_damageFalloffPercent / 100f);

        if (startTarget.TryGetComponent(out EnemyController currentTarget))
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

    private void DealDamage(EnemyController enemy, float damage)
    {
        float actualDamage = enemy.HealthComponent.TakeDamage(damage, this);
        TotalDamageDeal += actualDamage;
        AddExpirience(Mathf.RoundToInt(actualDamage));
    }

    private void ResetCooldown() => _fireCooldown = 60f / _fireRate;

    private void StunEnemy(EnemyController enemy)
    {
        if (enemy.TryGetComponent(out IStunnable stunnable))
        {
            stunnable.Stun(_stunDuration);
        }
    }

    private EnemyController FindClosestEnemy(Vector3 fromPosition, List<EnemyController> excludedEnemies)
    {
        Collider[] colliders = Physics.OverlapSphere(fromPosition, _chainRadius);

        EnemyController closest = null;
        float minDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyController enemy))
            {
                if (excludedEnemies.Contains(enemy))
                    continue;

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
            new("Damage\n", _damage.ToString("F2")),
            new("Rotation Speed\n", _rotationSpeed.ToString("F2")),
            new("Radius\n", _visionRange.ToString("F2")),
            new("Fire Rate (min)\n", $"{_fireRate:F2}"),
            new("Damage Falloff (%)\n", _damageFalloffPercent.ToString("F2")),
            new("Max Chains\n", _maxChainCount.ToString("F2")),
            new("Stun Duration (s)\n", $"{_stunDuration:F2}")
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = _damage * _upgradeConfig.DamageMultiplier;
        float futureFireRate = _fireRate * _upgradeConfig.FireRateMultiplier;
        float futureRadius = _visionRange * _upgradeConfig.VisionRangeMultiplier;
        float futureDamageFalloff = _damageFalloffPercent * _upgradeConfig.DamageFalloffMultiplier;
        float futureStunDuration = _stunDuration * _upgradeConfig.StunDurationMultiplier;

        string separator = "<color=#FFFFFF>>>></color>";
        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new("Damage\n", $"{_damage:F2} {separator} {upgradeColor}{futureDamage:F2}</color>"),
            new("Rotation Speed\n", _rotationSpeed.ToString("F2")),
            new("Radius\n", $"{_visionRange:F2} {separator} {upgradeColor}{futureRadius:F2}</color>"),
            new("Fire Rate (min)\n", $"{_fireRate:F2} {separator} {upgradeColor}{futureFireRate:F2}</color>"),
            new("Damage Falloff (%)\n", $"{_damageFalloffPercent:F2} {separator} {upgradeColor}{futureDamageFalloff:F2}</color>"),
            new("Max Chains\n", _maxChainCount.ToString("F2")),
            new("Stun Duration (s)\n", $"{_stunDuration:F2} {separator} {upgradeColor} {futureStunDuration:F2}</color>")
        };
    }
}