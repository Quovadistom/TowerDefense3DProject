using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BasicEnemy : Poolable
{
    public float Speed = 5f;
    public int StartingHealth = 100;
    public Transform EnemyMiddle;

    private int m_waypointIndex = 0;
    private Transform m_target;
    private LevelService m_levelService;
    private float m_currentHealth;

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
        ResetObject();
    }

    private void OnEnable()
    {
        SetWaypoint(m_waypointIndex);
    }

    private void Update()
    {
        Vector3 direction = m_target.position - transform.position;
        transform.Translate(direction.normalized * Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, m_target.position) <= 0.02f)
        {
            m_waypointIndex++;
            SetWaypoint(m_waypointIndex);
        }
    }

    private void SetWaypoint(int waypointIndex)
    {
        if (m_levelService.Waypoints.Count > waypointIndex)
        {
            m_target = m_levelService.Waypoints[waypointIndex];
        }
        else
        {
            m_levelService.ReduceHealth();
            m_poolingService.ReturnPooledObject(this);
        }
    }

    public override void ResetObject()
    {
        base.ResetObject();
        m_waypointIndex = 0;
        m_currentHealth = StartingHealth;
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth <= 0)
        {
            m_poolingService.ReturnPooledObject(this);
            ResetObject();
        }
    }
}