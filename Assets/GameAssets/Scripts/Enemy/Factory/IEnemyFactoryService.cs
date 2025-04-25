using UnityEngine;

public interface IEnemyFactoryService
{
    void RegisterFactory(EnemyType type, IEnemyFactory factory);
    IEnemy CreateEnemy(EnemyType type, Transform spawnPoint);
    IEnemy CreateEnemyWithParent(EnemyType type, Transform parent);
    IEnemy CreateEnemy(EnemyType type);
}