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
    [SerializeField] private List<EnemyWaveType> _enemyTypes = new();

    [Header("Wave Settings")]
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
    private IEnemyFactoryService _enemyFactoryService;

    private void Awake()
    {
        _enemyFactoryService = Bootstrapper.ServiceLocator.Resolve<IEnemyFactoryService>();

        StartCoroutine(SpawnWaveCoroutine());
    }

    private void OnEnable()
    {
        Bootstrapper.Instance.EventBus.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        Bootstrapper.Instance.EventBus.OnGameOver -= GameOver;
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

        List<EnemyWaveType> selectedTypes = GetEnemyTypesForWave(_currentWave);

        float spawnRate = Mathf.Max(0.1f, _baseSpawnRate - _spawnRateDecreasePerWave * _currentWave);

        for (int i = 0; i < enemiesCount; i++)
        {
            EnemyWaveType typeToSpawn = selectedTypes[Random.Range(0, selectedTypes.Count)];
            SpawnEnemy(typeToSpawn);
            yield return new WaitForSeconds(spawnRate);
        }

        _isSpawning = false;
    }

    private void SpawnEnemy(EnemyWaveType type)
    {
        IEnemy enemy = _enemyFactoryService.CreateEnemy(type.EnemyType);

        if (enemy != null)
        {
            float health = type.BaseHealth * Mathf.Pow(type.HealthGrowthFactor, _currentWave);
            float speed = type.BaseSpeed * Mathf.Pow(type.SpeedGrowthFactor, _currentWave);
            int reward = Mathf.RoundToInt(type.BaseReward * Mathf.Pow(type.RewardGrowthFactor, _currentWave));

            enemy.Initialize(reward, health);
            enemy.InitializeSpeed(speed);
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

    private List<EnemyWaveType> GetEnemyTypesForWave(int wave)
    {
        List<EnemyWaveType> availableTypes = new();

        int typeCount = Mathf.Min(1 + wave / _wavesPerTypeChange, _enemyTypes.Count);

        for (int i = 0; i < typeCount; i++)
        {
            availableTypes.Add(_enemyTypes[i]);
        }

        return availableTypes;
    }

    private void UpdateWaveUI()
    {
        _waveText.text = _currentWave.ToString();
    }

    private void UpdateNextWaveTimerUI(float timeLeft)
    {
        _nextWaveTimerText.text = timeLeft.ToString("F1");
    }
}