using UnityEngine;

public class WeakEnemyFactory : IEnemyFactory
{
    private readonly WeakEnemy _prefab;

    public WeakEnemyFactory(WeakEnemy prefab)
    {
        _prefab = prefab;
    }

    public IEnemy CreateEnemy(Transform spawnPoint)
    {
        WeakEnemy enemyObj = Object.Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        return enemyObj.GetComponent<IEnemy>();
    }

    public IEnemy CreateEnemy()
    {
        WeakEnemy enemyObj = Object.Instantiate(_prefab);
        return enemyObj.GetComponent<IEnemy>();
    }

    public IEnemy CreateEnemyWithParent(Transform parent)
    {
        WeakEnemy enemyObj = Object.Instantiate(_prefab, parent);
        return enemyObj.GetComponent<IEnemy>();
    }
}