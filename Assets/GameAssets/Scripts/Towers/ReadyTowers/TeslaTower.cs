using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TeslaTower : TowerBase, ITowerStats
{
    [Header("Tesla Tower Settings")]
    [SerializeField] private LineRenderer _lineRendererPrefab;

    [Space]
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _minDamage = 0.2f;
    [SerializeField, Range(0f, 100f)] private float _damageFalloffPercent = 30f;
    [SerializeField] private int _maxChainCount = 3;
    [SerializeField] private float _chainRadius = 5f;
    [SerializeField] private float _stunDuration = 0.1f;
    [SerializeField] private float _fireRate = 60f;
    [SerializeField] private int _lightningCount = 2;
    
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

            for (int i = 0; i < _maxChainCount; i++)
            {
                if (currentTarget == null)
                    break;

                DealDamage(currentTarget, currentDamage);
                StunEnemy(currentTarget);

                hitEnemies.Add(currentTarget);
                points.Add(currentTarget.transform.position);

                currentTarget = FindClosestEnemy(currentTarget.transform.position, hitEnemies);

                currentDamage *= damageFalloff;
                currentDamage = Mathf.Max(currentDamage, _minDamage);

                yield return new WaitForSeconds(0.05f);
            }

            if (points.Count >= 2)
            {
                SpawnLightning(points);
            }
        }
    }

    private void SpawnLightning(List<Vector3> points)
    {
        for (int l = 0; l < _lightningCount; l++)
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

            currentLineRenderer.transform
                .DOShakePosition(0.2f, 0.2f, 10, 90, false, true)
                .SetEase(Ease.OutQuad);

            Material materialInstance = new(currentLineRenderer.material);
            currentLineRenderer.material = materialInstance;

            materialInstance
                .DOFade(0f, 0.2f)
                .SetDelay(0.2f)
                .OnComplete(() =>
                {
                    Destroy(currentLineRenderer.gameObject);
                });
        }
    }

    private void DealDamage(EnemyController enemy, float damage)
    {
        enemy.GetHealthComponent().TakeDamage(damage);
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
            new("Damage", _damage.ToString()),
            new("Min Damage", _minDamage.ToString()),
            new("Damage Falloff (%)", _damageFalloffPercent.ToString()),
            new("Max Chains", _maxChainCount.ToString()),
            new("Stun Duration", _stunDuration.ToString()),
            new("Fire Rate", $"{_fireRate}/min"),
            new("Rotation Speed", _rotationSpeed.ToString()),
            new("Radius", _visionRange.ToString())
        };
    }
}