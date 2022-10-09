using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TurretFactory
{
    readonly DiContainer _container;
    readonly List<Object> _prefabs;

    public TurretFactory(
        List<Object> prefabs,
        DiContainer container)
    {
        _container = container;
        _prefabs = prefabs;
    }

    public TurretEnemyHandler Create<T>()
        where T : TurretEnemyHandler
    {
        var prefab = _prefabs.OfType<T>().Single();
        return _container.InstantiatePrefabForComponent<T>(prefab);
    }
}
