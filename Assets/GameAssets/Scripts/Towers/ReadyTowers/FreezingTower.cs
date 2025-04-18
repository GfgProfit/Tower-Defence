using System.Collections.Generic;
using UnityEngine;

public class FreezingTower : TowerBase, ITowerStats
{
    [Header("Freezing Tower Settings")]
    [SerializeField] private float _freezeMultiplier = 1.2f;
    [SerializeField] private float _freezeDuration = 2f;

    protected override void Update()
    {
        base.Update();

        ApplyFreezeEffect();
    }

    private void ApplyFreezeEffect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _visionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyController enemy))
            {
                enemy.ApplySlow(_freezeMultiplier, _freezeDuration);
            }
        }
    }

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new("Freeze Multiplier", _freezeMultiplier.ToString()),
            new("Freeze Duration", _freezeDuration.ToString()),
            new("Radius", _visionRange.ToString())
        };
    }
}