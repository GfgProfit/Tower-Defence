using UnityEngine;

public interface IEnemyFactory
{
    IEnemy CreateEnemy(Transform spawnPoint);
    IEnemy CreateEnemy();
}