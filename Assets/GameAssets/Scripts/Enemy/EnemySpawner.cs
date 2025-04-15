using UnityEngine;
using System.Collections;
using GameAssets.Global.Core;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private PathPoints _path;
    
    [Space]
    [SerializeField] private float _spawnRate = 2.0f;

    private bool _isGameOver = false;

    private void Awake()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void OnEnable()
    {
        GameController.Instance.EventBus.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameController.Instance.EventBus.OnGameOver -= GameOver;
    }

    private IEnumerator SpawnEnemies()
    {
        while (_isGameOver == false)
        {
            EnemyController enemy = Instantiate(_enemyPrefab, _path.GetWaypoints()[0].position, Quaternion.identity);

            enemy.SetPath(_path.GetWaypoints());

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void GameOver() => _isGameOver = true;
}