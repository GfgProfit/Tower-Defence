using UnityEngine;

[CreateAssetMenu(fileName = "Tesla Upgrade Config", menuName = "Towers/Tesla Upgrade Config", order = 0)]
public class TeslaUpgradeConfig : ScriptableObject, ITowerUpgradeConfig
{
    [Header("Upgrade Multipliers")]
    [SerializeField] private float _damageMultiplier = 1.2f;
    [SerializeField] private float _fireRateMultiplier = 1.1f;
    [SerializeField] private float _visionRangeMultiplier = 1.05f;
    [SerializeField] private float _damageFalloffMultiplier = 0.8f;
    [SerializeField] private float _stunDurationMultiplier = 1.2f;

    public float DamageMultiplier => _damageMultiplier;
    public float FireRateMultiplier => _fireRateMultiplier;
    public float VisionRangeMultiplier => _visionRangeMultiplier;
    public float DamageFalloffMultiplier => _damageFalloffMultiplier;
    public float StunDurationMultiplier => _stunDurationMultiplier;

    public void ApplyUpgrade(TowerBase towerBase)
    {
        if (towerBase is TeslaTower teslaTower)
        {
            teslaTower.UpgradeTeslaTower(
                DamageMultiplier,
                FireRateMultiplier,
                VisionRangeMultiplier,
                DamageFalloffMultiplier,
                StunDurationMultiplier
            );
        }
    }
}