using UnityEngine;
using System.Collections;
using GameAssets.Global.Core;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnerTransform;
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private PathPoints _path;
    
    [Space]
    [SerializeField] private float _spawnRate = 2.0f;

    private bool _isGameOver = false;

    private void Start()
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

            enemy.name = "Enemy";
            enemy.SetPath(_path.GetWaypoints());

            if (_spawnerTransform != null)
            {
                _spawnerTransform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 1);
            }

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void GameOver() => _isGameOver = true;
}