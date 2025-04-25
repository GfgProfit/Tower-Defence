using UnityEngine;

public interface IEnemyFactory
{
    IEnemy CreateEnemy(Transform spawnPoint);
    IEnemy CreateEnemyWithParent(Transform parent);
    IEnemy CreateEnemy();
}