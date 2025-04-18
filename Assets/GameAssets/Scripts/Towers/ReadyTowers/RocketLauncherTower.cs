using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RocketLauncherTower : TowerBase, ITowerStats
{
    [Header("Rocket Launcher Tower Settings")]
    [SerializeField] private ParticleSystem _rocketExplosionPrefab;

    [Space]
    [SerializeField] private Rocket[] _rockets;

    [Space]
    [SerializeField] private float _damagePerRocket = 1.5f;
    [SerializeField] private float _delayBetweenRocketLaunches = 0.2f;
    [SerializeField] private float _reloadTime = 2.0f;
    [SerializeField] private float _afterReloadDelay = 0.5f;
    [SerializeField] private float _flyDuration = 1.0f;

    private int _nextRocketIndex = 0;
    private float _launchTimer = 0f;
    private float _reloadTimer = 0f;
    private float _afterReloadTimer = 0f;
    private bool _isReloading = false;
    private bool _isWaitingAfterReload = false;

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
        foreach (Rocket rocket in _rockets)
        {
            if (rocket == null) continue;

            rocket.RocketReadyToLaunch(false);
            rocket.gameObject.SetActive(true);

            AnimateRocketAppearance(rocket);
        }
    }

    private void LaunchNextRocket()
    {
        Rocket rocket = _rockets[_nextRocketIndex];

        if (rocket == null)
            return;

        LaunchRocket(rocket, _currentTarget);

        _nextRocketIndex++;
    }

    public void LaunchRocket(Rocket rocket, Transform target)
    {
        if (!CanLaunchRocket(rocket, target))
        {
            return;
        }

        PrepareRocket(rocket);

        MoveRocketToTarget(rocket, target.position);
    }

    private bool CanLaunchRocket(Rocket rocket, Transform target)
    {
        return target != null && !rocket.IsRocketReadyToLaunch;
    }

    private void PrepareRocket(Rocket rocket)
    {
        rocket.RocketReadyToLaunch(true);
        rocket.gameObject.SetActive(true);
    }

    private void MoveRocketToTarget(Rocket rocket, Vector3 targetPosition)
    {
        Transform rocketTransform = rocket.transform;

        rocketTransform
            .DOMove(targetPosition, _flyDuration)
            .SetEase(Ease.InQuart)
            .OnComplete(() =>
            {
                rocket.TryDealDamage(_damagePerRocket);

                ParticleSystem explosion = Instantiate(_rocketExplosionPrefab, rocket.transform.position, Quaternion.identity);

                ResetRocket(rocket);
            })
            .SetLink(rocket.gameObject, LinkBehaviour.KillOnDestroy);

        rocketTransform.LookAt(targetPosition);
    }

    private void AnimateRocketAppearance(Rocket rocket)
    {
        rocket.transform.localScale = Vector3.zero;

        rocket.transform.DOScale(Vector3.one, _afterReloadDelay - 0.1f).SetEase(Ease.InSine);
    }

    private void ResetRocket(Rocket rocket)
    {
        rocket.transform.SetLocalPositionAndRotation(rocket.GetStartPoint(), rocket.GetStartRotation());
        rocket.RocketReadyToLaunch(false);
        rocket.gameObject.SetActive(false);
    }

    public List<StatData> GetStats()
    {
        return new List<StatData>
        {
            new("Damage per Rocket", _damagePerRocket.ToString()),
            new("Delay Between Rockets", _delayBetweenRocketLaunches.ToString()),
            new("Reload Time", _reloadTime.ToString()),
            new("After Reload Delay", _afterReloadDelay.ToString()),
            new("Fly To Target Duration", _flyDuration.ToString()),
            new("Radius", _visionRange.ToString())
        };
    }
}