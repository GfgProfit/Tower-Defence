using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Path path;
    public float spawnRate = 2.0f;

    private bool _isGameOvered = false;

    private void Start()
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
            Enemy enemy = Instantiate(enemyPrefab);
            enemy.SetPath(path.GetWaypoints());

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void GameOver() => _isGameOvered = true;
}