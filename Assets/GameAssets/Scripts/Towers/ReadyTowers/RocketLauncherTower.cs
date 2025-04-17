using DG.Tweening;
using UnityEngine;

public class RocketLauncherTower : TowerBase
{
    [Header("Rocket Launcher Tower Settings")]
    [SerializeField] private ParticleSystem _rocketExplosionPrefab;

    [Space]
    [SerializeField] private Rocket[] _rockets;

    [Space]
    [SerializeField] private float _damagePerRocket = 1.5f;
    [SerializeField] private float _delayBetweenLaunches = 0.2f; // Задержка между запуском ракет
    [SerializeField] private float _riseHeight = 5f; // Средняя высота основной траектории
    [SerializeField] private float _riseDuration = 0.5f; // Время подъема к контрольной точке
    [SerializeField] private float _flyDuration = 1.0f; // Время полета до цели
    [SerializeField] private float _initialRiseHeight = 2f; // Маленький подъем сразу после старта
    [SerializeField] private float _lookAheadTime = 0.25f; // Плавное следование взгляда вдоль пути

    private int _nextRocketIndex = -1;
    private float _cooldownBetweenLaunches = 0;

    protected override void Update()
    {
        base.Update();

        UpdateRocketLaunchTimer();
    }

    private void UpdateRocketLaunchTimer()
    {
        if (_cooldownBetweenLaunches < _delayBetweenLaunches)
        {
            _cooldownBetweenLaunches += Time.deltaTime;

            return;
        }

        _cooldownBetweenLaunches = 0f;
        TryLaunchNextRocket();
    }

    private void TryLaunchNextRocket()
    {
        Rocket nextRocket = GetNextRocket();
        LaunchRocket(nextRocket, _currentTarget);
    }

    private Rocket GetNextRocket()
    {
        _nextRocketIndex = (_nextRocketIndex + 1) % _rockets.Length;

        return _rockets[_nextRocketIndex];
    }

    public void LaunchRocket(Rocket rocket, Transform target)
    {
        if (!CanLaunchRocket(rocket, target))
        {
            return;
        }

        PrepareRocket(rocket);

        Vector3 startPoint = rocket.transform.position;
        Vector3 risePoint = startPoint + Vector3.up * _initialRiseHeight;
        Vector3[] path = CalculateRocketPath(risePoint, target.position);

        MoveRocketAlongPath(rocket, path);
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

    private Vector3[] CalculateRocketPath(Vector3 startPoint, Vector3 targetPosition)
    {
        float randomRiseHeight = _riseHeight + Random.Range(-1f, 1f);
        Vector3 controlPoint = startPoint + Vector3.up * randomRiseHeight;
        controlPoint += (targetPosition - startPoint) * 0.5f;

        Vector3 randomOffset = new(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
        Vector3 finalTargetPosition = targetPosition + randomOffset;

        return new Vector3[] { startPoint, controlPoint, finalTargetPosition };
    }

    private void MoveRocketAlongPath(Rocket rocket, Vector3[] path)
    {
        Transform rocketTransform = rocket.transform;

        DOTween.Sequence()
            .Append(
                rocketTransform.DOPath(path, _riseDuration + _flyDuration, PathType.CatmullRom)
                    .SetEase(Ease.InQuint)
                    .SetLookAt(_lookAheadTime)
            )
            .OnComplete(() =>
            {
                rocket.TryDealDamage(_damagePerRocket);

                ParticleSystem explosion = Instantiate(_rocketExplosionPrefab, rocket.transform.position, Quaternion.identity);

                Destroy(explosion, 0.2f);

                ResetRocket(rocket);
            })
            .SetLink(rocket.gameObject, LinkBehaviour.KillOnDestroy);
    }

    private void ResetRocket(Rocket rocket)
    {
        rocket.transform.SetLocalPositionAndRotation(rocket.GetStartPoint(), rocket.GetStartRotation());
        rocket.RocketReadyToLaunch(false);
        rocket.gameObject.SetActive(false);
    }
}