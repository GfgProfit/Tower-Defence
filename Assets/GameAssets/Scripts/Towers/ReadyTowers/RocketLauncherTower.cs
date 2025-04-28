using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherTower : TowerBase, ITowerStats
{
    [Header("Rocket Launcher Tower Settings")]
    [SerializeField] private ParticleSystem _rocketExplosionPrefab;
    [SerializeField] private RocketProvider _rocketProvider;
    [SerializeField] private RocketLauncherUpgradeConfig _upgradeConfig;

    [Space]
    [SerializeField] private float _damagePerRocket = 1.5f;
    [SerializeField] private float _delayBetweenRocketLaunches = 0.2f;
    [SerializeField] private float _reloadTime = 2.0f;
    [SerializeField] private float _afterReloadDelay = 0.5f;
    [SerializeField] private float _flyDuration = 1.0f;

    private IRocket[] _rockets;

    private int _nextRocketIndex = 0;
    private float _launchTimer = 0f;
    private float _reloadTimer = 0f;
    private float _afterReloadTimer = 0f;
    private bool _isReloading = false;
    private bool _isWaitingAfterReload = false;

    private void Start()
    {
        _rockets = _rocketProvider.GetAvailableRockets();

        foreach (IRocket rocket in _rockets)
        {
            rocket.SetDamageCallback(OnRocketDamageDealt);
            rocket.SeeAddExpirienceCallback(OnAddExpirience);
            rocket.SetOwner(this);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_currentTarget == null)
            return;

        if (_isReloading)
        {
            UpdateReloadTimer();
        }
        else if (_isWaitingAfterReload)
        {
            UpdateAfterReloadTimer();
        }
        else
        {
            UpdateRocketLaunchTimer();
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        _upgradeConfig.ApplyUpgrade(this);
    }

    public void UpgradeRocketLauncherTower(float damageMult, float rangeMult, float rotationSpeedMult, float delayBetweenRocketLaunchesMult, float reloadTimeMult, float afterReloadDelayMult, float flyDurationMult)
    {
        _damagePerRocket *= damageMult;
        _visionRange *= rangeMult;
        _rotationSpeed *= rotationSpeedMult;
        _delayBetweenRocketLaunches *= delayBetweenRocketLaunchesMult;
        _reloadTime *= reloadTimeMult;
        _afterReloadTimer *= afterReloadDelayMult;
        _flyDuration *= flyDurationMult;
    }

    protected override void UpgradeByAutoLevel()
    {
        _damagePerRocket *= 1.06f;
        _visionRange *= 1.01f;
        _rotationSpeed *= 1.01f;
        _delayBetweenRocketLaunches *= 0.95f;
        _reloadTime *= 0.95f;
        _afterReloadTimer *= 0.95f;
        _flyDuration *= 0.95f;
    }

    private void UpdateRocketLaunchTimer()
    {
        if (_nextRocketIndex >= _rockets.Length)
        {
            StartReloading();
            return;
        }

        _launchTimer += Time.deltaTime;

        if (_launchTimer >= _delayBetweenRocketLaunches)
        {
            _launchTimer = 0f;
            LaunchNextRocket();
        }
    }

    private void UpdateReloadTimer()
    {
        _reloadTimer += Time.deltaTime;

        if (_reloadTimer >= _reloadTime)
        {
            _reloadTimer = 0f;
            _isReloading = false;
            StartAfterReloadDelay();
        }
    }

    private void StartReloading()
    {
        _isReloading = true;
        _reloadTimer = 0f;
    }

    private void StartAfterReloadDelay()
    {
        PrepareAllRockets();
        _isWaitingAfterReload = true;
        _afterReloadTimer = 0f;
    }

    private void UpdateAfterReloadTimer()
    {
        _afterReloadTimer += Time.deltaTime;

        if (_afterReloadTimer >= _afterReloadDelay)
        {
            _afterReloadTimer = 0f;
            _isWaitingAfterReload = false;
            _nextRocketIndex = 0;
        }
    }

    private void PrepareAllRockets()
    {
        foreach (IRocket rocket in _rockets)
        {
            if (rocket == null)
                continue;

            rocket.PrepareForLaunch(_afterReloadDelay);
        }
    }

    private void OnRocketDamageDealt(float damage)
    {
        TotalDamageDeal += damage;
    }

    private void OnAddExpirience(float exp)
    {
        AddExpirience(exp);
    }

    private void LaunchNextRocket()
    {
        IRocket rocket = _rockets[_nextRocketIndex];

        if (rocket == null)
        {
            return;
        }

        LaunchRocket(rocket, _currentTarget);

        _nextRocketIndex++;
    }

    public void LaunchRocket(IRocket rocket, Transform target)
    {
        if (!CanLaunchRocket(rocket, target))
        {
            return;
        }

        rocket.Launch(
            targetPosition: target.position,
            flyDuration: _flyDuration,
            damage: _damagePerRocket,
            explosionPrefab: _rocketExplosionPrefab
        );
    }

    private bool CanLaunchRocket(IRocket rocket, Transform target)
    {
        return target != null && !rocket.IsRocketReadyToLaunch;
    }

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new("Damage/Rocket", _damagePerRocket.ToString("F2")),
            new("Rotation Speed", _rotationSpeed.ToString("F2")),
            new("Radius", _visionRange.ToString("F2")),
            new("Delay/Rocket", _delayBetweenRocketLaunches.ToString("F2")),
            new("Reload Time", _reloadTime.ToString("F2")),
            new("Rocket Fly Duration", _flyDuration.ToString("F2")),
            new("After Reload Delay", _afterReloadDelay.ToString("F2"))
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = (_damagePerRocket * _upgradeConfig.DamageMultiplier) - _damagePerRocket;
        float futureRadius = (_visionRange * _upgradeConfig.VisionRangeMultiplier) - _visionRange;
        float futureRotationSpeed = (_rotationSpeed * _upgradeConfig.RotationSpeedMultiplier) - _rotationSpeed;
        float futureDelayBetweenRockets = (_delayBetweenRocketLaunches * _upgradeConfig.DelayBetweenRocketLaunchesMultiplier) - _delayBetweenRocketLaunches;
        float futureReloadTime = (_reloadTime * _upgradeConfig.ReloadTimeMultiplier) - _reloadTime;
        float futureFlyDuration = (_flyDuration * _upgradeConfig.FlyDurationMultiplier) - _flyDuration;

        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new("Damage/Rocket", $"{_damagePerRocket:F2}<b><size=20>{upgradeColor} +{futureDamage:F2}</b></size></color>"),
            new("Rotation Speed", $"{_rotationSpeed:F2}<b><size=20>{upgradeColor} +{futureRotationSpeed:F2}</b></size></color>"),
            new("Radius", $"{_visionRange:F2}<b><size=20>{upgradeColor} +{futureRadius:F2}</b></size></color>"),
            new("Delay/Rocket", $"{_delayBetweenRocketLaunches:F2}<b><size=20>{upgradeColor} {futureDelayBetweenRockets:F2}</b></size></color>"),
            new("Reload Time", $"{_reloadTime:F2}<b><size=20>{upgradeColor} {futureReloadTime:F2}</b></size></color>"),
            new("Rocket Fly Duration", $"{_flyDuration:F2}<b><size=20>{upgradeColor} {futureFlyDuration:F2}</b></size></color>"),
            new("After Reload Delay", _afterReloadDelay.ToString("F2"))
        };
    }
}
