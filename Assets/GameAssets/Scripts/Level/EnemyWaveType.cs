using UnityEngine;

[System.Serializable]
public class EnemyWaveType
{
    [SerializeField] private string _name;
    [SerializeField] private EnemyBase _prefab;
    [SerializeField] private float _baseHealth;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private int _baseReward;
    [SerializeField] private float _healthGrowthFactor = 1.05f;
    [SerializeField] private float _speedGrowthFactor = 1.02f;
    [SerializeField] private float _rewardGrowthFactor = 1.04f;

    public string Name => _name;
    public EnemyBase EnemyPrefab => _prefab;
    public float BaseHealth => _baseHealth;
    public float BaseSpeed => _baseSpeed;
    public int BaseReward => _baseReward;
    public float HealthGrowthFactor => _healthGrowthFactor;
    public float SpeedGrowthFactor => _speedGrowthFactor;
    public float RewardGrowthFactor => _rewardGrowthFactor;
}