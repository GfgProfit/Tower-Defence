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
            {
                continue;
            }

            rocket.PrepareForLaunch(_afterReloadDelay);
        }
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
            new("Damage per Rocket\n", _damagePerRocket.ToString("F2")),
            new("Rockets Delay (s)\n", _delayBetweenRocketLaunches.ToString("F2")),
            new("Reload Time (s)\n", _reloadTime.ToString("F2")),
            new("After Reload Delay (s)\n", _afterReloadDelay.ToString("F2")),
            new("Fly To Target (s)\n", _flyDuration.ToString("F2")),
            new("Rotation Speed\n", _rotationSpeed.ToString("F2")),
            new("Radius\n", _visionRange.ToString("F2"))
        };
    }

    public List<StatData> GetStatsAfterUpgrade()
    {
        float futureDamage = _damagePerRocket * _upgradeConfig.DamageMultiplier;
        float futureRadius = _visionRange * _upgradeConfig.VisionRangeMultiplier;
        float futureRotationSpeed = _rotationSpeed * _upgradeConfig.RotationSpeedMultiplier;
        float futureDelayBetweenRockets = _delayBetweenRocketLaunches * _upgradeConfig.DelayBetweenRocketLaunchesMultiplier;
        float futureReloadTime = _reloadTime * _upgradeConfig.ReloadTimeMultiplier;
        float futureAfterReloadDelay = _afterReloadDelay * _upgradeConfig.AfterReloadDelayMultiplier;
        float futureFlyDuration = _flyDuration * _upgradeConfig.FlyDurationMultiplier;

        string separator = "<color=#FFFFFF>>>></color>";
        string upgradeColor = $"<color={Utils.ColorToHex(Color.green)}>";

        return new List<StatData>
        {
            new("Damage per Rocket\n", $"{_damagePerRocket:F2} {separator} {upgradeColor}{futureDamage:F2}</color>"),
            new("Rockets Delay (s)\n", $"{_delayBetweenRocketLaunches:F2} {separator} {upgradeColor}{futureDelayBetweenRockets:F2}</color>"),
            new("Reload Time (s)\n", $"{_reloadTime:F2} {separator} {upgradeColor}{futureReloadTime:F2}</color>"),
            new("After Reload Delay (s)\n", $"{_afterReloadDelay:F2} {separator} {upgradeColor}{futureAfterReloadDelay:F2}</color>"),
            new("Fly To Target (s)\n", $"{_flyDuration:F2} {separator} {upgradeColor}{futureFlyDuration:F2}</color>"),
            new("Rotation Speed\n", $"{_rotationSpeed:F2} {separator} {upgradeColor}{futureRotationSpeed:F2}</color>"),
            new("Radius\n", $"{_visionRange:F2} {separator} {upgradeColor}{futureRadius:F2}</color>")
        };
    }
}
