using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Path path;
    public float spawnRate = 2.0f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Enemy enemy = Instantiate(enemyPrefab);
            enemy.SetPath(path.GetWaypoints());

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
