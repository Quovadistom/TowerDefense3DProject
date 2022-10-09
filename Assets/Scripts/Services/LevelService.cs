using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelService
{    
    private List<Transform> m_waypoints = new List<Transform>();
    public IReadOnlyList<Transform> Waypoints => m_waypoints;

    public event Action<int> HealthChanged;
    private int m_health;
    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            HealthChanged?.Invoke(m_health);
        } 
    }

    public void SetWaypoints(List<Transform> waypoints)
    {
        m_waypoints = waypoints;
    }

    public void StartLevel()
    {
        Health = 10;
    }

    public void ReduceHealth()
    {
        Health--;
    }
    
    public void IncreaseHealth()
    {
        Health++;
    }
}
