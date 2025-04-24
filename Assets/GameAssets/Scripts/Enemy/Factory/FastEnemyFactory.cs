using UnityEngine;

public class FastEnemyFactory : IEnemyFactory
{
    private readonly FastEnemy _prefab;

    public FastEnemyFactory(FastEnemy prefab)
    {
        _prefab = prefab;
    }

    public IEnemy CreateEnemy(Transform spawnPoint)
    {
        FastEnemy enemyObj = Object.Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        return enemyObj.GetComponent<IEnemy>();
    }

    public IEnemy CreateEnemy()
    {
        FastEnemy enemyObj = Object.Instantiate(_prefab);
        return enemyObj.GetComponent<IEnemy>();
    }
}