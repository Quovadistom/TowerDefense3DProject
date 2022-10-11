using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TurretFactory
{
    readonly DiContainer m_container;
    readonly List<Object> m_prefabs;

    public TurretFactory(
        List<Object> prefabs,
        DiContainer container)
    {
        m_container = container;
        m_prefabs = prefabs;
    }

    public TurretEnemyHandler Create<T>()
        where T : TurretEnemyHandler
    {
        var prefab = m_prefabs.OfType<T>().Single();
        return m_container.InstantiatePrefabForComponent<T>(prefab);
    }
}
