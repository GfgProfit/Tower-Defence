using UnityEngine;

[CreateAssetMenu(fileName = "Cannon Upgrade Config", menuName = "Towers/Cannon Upgrade Config", order = 0)]
public class CannonUpgradeConfig : ScriptableObject, ITowerUpgradeConfig
{
    [Header("Upgrade Multipliers")]
    [SerializeField] private float _damageMultiplier = 1.1f;
    [SerializeField] private float _fireRateMultiplier = 1.2f;
    [SerializeField] private float _visionRangeMultiplier = 1.1f;
    [SerializeField] private float _rotationSpeedMultiplier = 1.05f;

    public float DamageMultiplier => _damageMultiplier;
    public float FireRateMultiplier => _fireRateMultiplier;
    public float VisionRangeMultiplier => _visionRangeMultiplier;
    public float RotationSpeedMultiplier => _rotationSpeedMultiplier;

    public void ApplyUpgrade(TowerBase towerBase)
    {
        if (towerBase is CannonTower teslaTower)
        {
            teslaTower.UpgradeCannonTower(
                DamageMultiplier,
                FireRateMultiplier,
                VisionRangeMultiplier,
                RotationSpeedMultiplier
            );
        }
    }
}
