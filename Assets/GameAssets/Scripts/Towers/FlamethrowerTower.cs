using UnityEngine;

public class FlamethrowerTower : TowerBase
{
    [Header("Flamethrower Tower Settings")]
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private float _damagePerSecond = 10f;

    private bool _isEffectPlaying;

    protected override IAttackBehavior CreateAttackBehavior()
    {
        return new FlamethrowerAttack(_damagePerSecond);
    }

    protected override void StartEffect()
    {
        if (!_isEffectPlaying)
        {
            _fireParticles.Play();
            _isEffectPlaying = true;
        }
    }

    protected override void StopEffect()
    {
        if (_isEffectPlaying)
        {
            _fireParticles.Stop();
            _isEffectPlaying = false;
        }
    }
}