using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTower : TowerBase, ITowerStats
{
    [Header("Flamethrower Tower Settings")]
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private float _damagePerSecond = 10f;

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
        {
            return;
        }

        if (_currentTarget.TryGetComponent(out EnemyController enemy))
        {
            DealDamage(enemy);
        }
    }

    private void DealDamage(EnemyController enemy)
    {
        float damageAmount = _damagePerSecond * Time.deltaTime;
        enemy.GetHealthComponent().TakeDamage(damageAmount);
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
            new("Damage Per Second", _damagePerSecond.ToString()),
            new("Radius", _visionRange.ToString()),
            new("Rotation Speed", _rotationSpeed.ToString())
        };
    }
}