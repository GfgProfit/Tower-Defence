using UnityEngine;

[CreateAssetMenu(fileName = "Rocket Launcher Upgrade Config", menuName = "Towers/Rocket Launcher Upgrade Config", order = 0)]
public class RocketLauncherUpgradeConfig : ScriptableObject, ITowerUpgradeConfig
{
    [Header("Upgrade Multipliers")]
    [SerializeField] private float _damageMultiplier = 1.1f;
    [SerializeField] private float _visionRangeMultiplier = 1.1f;
    [SerializeField] private float _rotationSpeedMultiplier = 1.05f;
    [SerializeField] private float _delayBetweenRocketLaunchesMultiplier = 0.95f;
    [SerializeField] private float _reloadTimeMultiplier = 0.925f;
    [SerializeField] private float _afterReloadDelayMultiplier = 0.9f;
    [SerializeField] private float _flyDurationMultiplier = 0.9f;

    public float DamageMultiplier => _damageMultiplier;
    public float VisionRangeMultiplier => _visionRangeMultiplier;
    public float RotationSpeedMultiplier => _rotationSpeedMultiplier;
    public float DelayBetweenRocketLaunchesMultiplier => _delayBetweenRocketLaunchesMultiplier;
    public float ReloadTimeMultiplier => _reloadTimeMultiplier;
    public float AfterReloadDelayMultiplier => _afterReloadDelayMultiplier;
    public float FlyDurationMultiplier => _flyDurationMultiplier;

    public void ApplyUpgrade(TowerBase towerBase)
    {
        if (towerBase is RocketLauncherTower teslaTower)
        {
            teslaTower.UpgradeRocketLauncherTower(
                DamageMultiplier,
                VisionRangeMultiplier,
                RotationSpeedMultiplier,
                DelayBetweenRocketLaunchesMultiplier,
                ReloadTimeMultiplier,
                AfterReloadDelayMultiplier,
                FlyDurationMultiplier
            );
        }
    }
}
