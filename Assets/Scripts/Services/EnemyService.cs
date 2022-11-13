using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyService
{
    public WaypointCollection WaypointsCollection;
    private PoolingService m_poolingService;

    public EnemyService(PoolingService poolingService)
    {
        m_poolingService = poolingService;
    }

    public BasicEnemy CreateNewEnemy(BasicEnemy enemy, Vector3 position)
    {
        BasicEnemy newEnemy = (BasicEnemy)m_poolingService.GetPooledObject(enemy);
        newEnemy.transform.position = position;
        return newEnemy;
    }   
}