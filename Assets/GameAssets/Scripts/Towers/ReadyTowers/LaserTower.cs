using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase, ITowerStats
{
    [Header("Laser Tower Settings")]
    [SerializeField] private ParticleSystem _fireParticles;
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
                enemy.GetHealthComponent().TakeDamage(currentDamage);

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
            new("Damage Per Second", _damagePerSecond.ToString("F1")),
            new("Radius", _visionRange.ToString("F1")),
            new("Rotation Speed", _rotationSpeed.ToString("F1")),
            new("Damage Falloff (%)", _damageFalloffPercent.ToString("F0")),
            new("Max Enemies Hit", _maxEnemiesHit.ToString())
        };
    }
}
