using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyFactoryService
{
    private readonly Dictionary<EnemyType, IEnemyFactory> _factories = new();

    public void RegisterFactory(EnemyType type, IEnemyFactory factory)
    {
        if (!_factories.ContainsKey(type))
        {
            _factories.Add(type, factory);
        }
    }

    public IEnemy CreateEnemy(EnemyType type, Transform spawnPoint)
    {
        if (_factories.TryGetValue(type, out IEnemyFactory factory))
        {
            return factory.CreateEnemy(spawnPoint);
        }

        throw new ArgumentException($"No factory registered for type: {type}");
    }

    public IEnemy CreateEnemy(EnemyType type)
    {
        if (_factories.TryGetValue(type, out IEnemyFactory factory))
        {
            return factory.CreateEnemy();
        }

        throw new ArgumentException($"No factory registered for type: {type}");
    }
}