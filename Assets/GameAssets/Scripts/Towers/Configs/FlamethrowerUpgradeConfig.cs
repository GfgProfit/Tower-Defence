using UnityEngine;

[CreateAssetMenu(fileName = "Flamethrower Upgrade Config", menuName = "Towers/Flamethrower Upgrade Config", order = 0)]
public class FlamethrowerUpgradeConfig : ScriptableObject, ITowerUpgradeConfig
{
    [Header("Upgrade Multipliers")]
    [SerializeField] private float _damageMultiplier = 1.05f;
    [SerializeField] private float _visionRangeMultiplier = 1.1f;
    [SerializeField] private float _damageFalloffMultiplier = 0.8f;
    [SerializeField] private float _rotationSpeedMultiplier = 1.05f;

    public float DamageMultiplier => _damageMultiplier;
    public float VisionRangeMultiplier => _visionRangeMultiplier;
    public float DamageFalloffMultiplier => _damageFalloffMultiplier;
    public float RotationSpeedMultiplier => _rotationSpeedMultiplier;

    public void ApplyUpgrade(TowerBase towerBase)
    {
        if (towerBase is LaserTower teslaTower)
        {
            teslaTower.UpgradeFlamethrowerTower(
                DamageMultiplier,
                VisionRangeMultiplier,
                DamageFalloffMultiplier,
                RotationSpeedMultiplier
            );
        }
    }
}
