using System;
using UnityEngine;

public class TurretCloseRangeComponent : MonoBehaviour
{
    [SerializeField] private float m_fireRate;

    public event Action<float> FirerateChanged;

    public float Firerate
    {
        get => m_fireRate;
        set
        {
            m_fireRate = value;
            FirerateChanged?.Invoke(m_fireRate);
        }
    }
}