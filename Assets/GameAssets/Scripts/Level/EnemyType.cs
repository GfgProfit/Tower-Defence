using UnityEngine;

[System.Serializable]
public class EnemyType
{
    [SerializeField] private string _name;
    [SerializeField] private EnemyController _prefab;
    [SerializeField] private float _baseHealth;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private int _baseReward;

    public string Name => _name;
    public EnemyController EnemyPrefab => _prefab;
    public float BaseHealth => _baseHealth;
    public float BaseSpeed => _baseSpeed;
    public int BaseReward => _baseReward;
}