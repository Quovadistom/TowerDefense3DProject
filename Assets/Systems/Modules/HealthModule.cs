using System;
using UnityEngine;

[Serializable]
public class HealthModule : ModuleBase
{
    [SerializeField] private int m_health;

    public event Action<int> OnHealthChange;

    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            OnHealthChange?.Invoke(m_health);
        }
    }
}