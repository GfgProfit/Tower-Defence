using System.Collections;
using System.Collections.Generic;
using GameAssets.Global.Core;
using UnityEngine;
using TMPro;

public partial class WaveGenerator : MonoBehaviour
{
    [Header("Components Links")]
    [SerializeField] private PathPoints _pathPoints;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _nextWaveTimerText;

    [Header("Enemy Types")]
    [SerializeField] private List<EnemyType> _enemyTypes = new();

    [Header("Wave Settings")]
    [SerializeField] private float _healthGrowthFactor = 1.05f;
    [SerializeField] private float _speedGrowthFactor = 1.02f;
    [SerializeField] private float _rewardGrowthFactor = 1.04f;
    [SerializeField] private int _baseEnemiesCount = 5;
    [SerializeField] private float _enemiesGrowthRate = 1.2f;

    [Header("Spawn Settings")]
    [SerializeField] private float _timeBetweenWaves = 10f;
    [SerializeField] private float _baseSpawnRate = 1f;
    [SerializeField] private float _spawnRateDecreasePerWave = 0.02f;

    [Header("Enemy Type Progression")]
    [SerializeField] private int _wavesPerTypeChange = 10;

    private int _currentWave = 0;
    private int _aliveEnemies = 0;
    private bool _isSpawning = false;
    private bool _isGameOver = false;
    private Coroutine _nextWaveCoroutine;

    private void Awake()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        _isGameOver = true;
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        if (_isGameOver) yield break;

        _isSpawning = true;
        _currentWave++;

        UpdateWaveUI();

        int enemiesCount = CalculateEnemiesCount();
        _aliveEnemies = enemiesCount;

        List<EnemyType> selectedTypes = GetEnemyTypesForWave(_currentWave);

        float spawnRate = Mathf.Max(0.1f, _baseSpawnRate - _spawnRateDecreasePerWave * _currentWave);

        for (int i = 0; i < enemiesCount; i++)
        {
            EnemyType typeToSpawn = selectedTypes[UnityEngine.Random.Range(0, selectedTypes.Count)];
            SpawnEnemy(typeToSpawn);
            yield return new WaitForSeconds(spawnRate);
        }

        Debug.Log($"Spawned Wave {_currentWave} with {enemiesCount} enemies of types {string.Join("/", selectedTypes.ConvertAll(t => t.Name))}");

        _isSpawning = false;
    }

    private void SpawnEnemy(EnemyType type)
    {
        EnemyController enemy = Instantiate(type.EnemyPrefab, _pathPoints.GetWaypoints()[0].position, Quaternion.identity);

        if (enemy != null)
        {
            float health = type.BaseHealth * Mathf.Pow(_healthGrowthFactor, _currentWave);
            float speed = type.BaseSpeed * Mathf.Pow(_speedGrowthFactor, _currentWave);
            int reward = Mathf.RoundToInt(type.BaseReward * Mathf.Pow(_rewardGrowthFactor, _currentWave));

            enemy.Initialize(speed, reward, health);
            enemy.name = type.Name;
            enemy.SetPath(_pathPoints.GetWaypoints());

            enemy.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        _aliveEnemies--;

        if (_aliveEnemies <= 0 && !_isGameOver)
        {
            _nextWaveCoroutine ??= StartCoroutine(StartNextWaveAfterDelay());
        }
    }

    private IEnumerator StartNextWaveAfterDelay()
    {
        float timer = _timeBetweenWaves;
        while (timer > 0f)
        {
            UpdateNextWaveTimerUI(timer);
            timer -= Time.deltaTime;
            yield return null;
        }

        UpdateNextWaveTimerUI(0f);

        if (!_isGameOver && !_isSpawning)
        {
            StartCoroutine(SpawnWaveCoroutine());
        }

        _nextWaveCoroutine = null;
    }

    private int CalculateEnemiesCount()
    {
        int enemiesCount;

        if (_currentWave <= 20)
        {
            enemiesCount = _baseEnemiesCount + Mathf.RoundToInt(_currentWave * _enemiesGrowthRate);
        }
        else
        {
            enemiesCount = _baseEnemiesCount + Mathf.RoundToInt(20 * _enemiesGrowthRate + (_currentWave - 20) * _enemiesGrowthRate * 1.5f);
        }

        enemiesCount = Mathf.Clamp(enemiesCount, 0, 100);

        return enemiesCount;
    }

    private List<EnemyType> GetEnemyTypesForWave(int wave)
    {
        List<EnemyType> availableTypes = new();

        int typeCount = Mathf.Min(1 + wave / _wavesPerTypeChange, _enemyTypes.Count);

        for (int i = 0; i < typeCount; i++)
        {
            availableTypes.Add(_enemyTypes[i]);
        }

        return availableTypes;
    }

    private void UpdateWaveUI()
    {
        _waveText.text = $"Wave: {_currentWave}";
    }

    private void UpdateNextWaveTimerUI(float timeLeft)
    {
        _nextWaveTimerText.text = $"Next Wave In: {timeLeft:F1}s";
    }
}