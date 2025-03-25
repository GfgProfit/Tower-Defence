using UnityEngine;
using System.Collections;
using GameAssets.Global.Core;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Path _path;
    [SerializeField] private float _spawnRate = 2.0f;

    private bool _isGameOvered = false;

    private void Awake()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void OnEnable()
    {
        GameManager.Instance.EventBus.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameManager.Instance.EventBus.OnGameOver -= GameOver;
    }

    private IEnumerator SpawnEnemies()
    {
        while (_isGameOvered == false)
        {
            Enemy enemy = Instantiate(_enemyPrefab);
            enemy.SetPath(_path.GetWaypoints());

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void GameOver() => _isGameOvered = true;
}