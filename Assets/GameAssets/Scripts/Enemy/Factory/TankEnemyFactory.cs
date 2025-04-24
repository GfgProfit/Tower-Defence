using UnityEngine;

public class TankEnemyFactory : IEnemyFactory
{
    private readonly TankEnemy _prefab;

    public TankEnemyFactory(TankEnemy prefab)
    {
        _prefab = prefab;
    }

    public IEnemy CreateEnemy(Transform spawnPoint)
    {
        TankEnemy enemyObj = Object.Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        return enemyObj.GetComponent<IEnemy>();
    }

    public IEnemy CreateEnemy()
    {
        TankEnemy enemyObj = Object.Instantiate(_prefab);
        return enemyObj.GetComponent<IEnemy>();
    }
}