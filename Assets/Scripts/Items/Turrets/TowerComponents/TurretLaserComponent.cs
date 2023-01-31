using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretLaserComponent : ChangeVisualComponent, ITowerComponent
{
    [SerializeField] private TurretRangeComponent m_turretRangeComponent;
    [SerializeField] private LaserSpawnPoints m_laserSpawnPoints;

    [SerializeField] private float m_damageRate;
    [SerializeField] private float m_laserLength;
    [SerializeField] private float m_laserDamage;

    public event Action<LaserSpawnPoints> LaserSpawnPointsChanged;
    public event Action<float> DamageRateChanged;
    public event Action<float> LaserLengthChanged;
    public event Action<float> LaserDamageChanged;

    public LaserSpawnPoints LaserSpawnPoints
    {
        get => m_laserSpawnPoints;
        set
        {
            Visual = value.transform;
            m_laserSpawnPoints = Visual.GetComponent<LaserSpawnPoints>();
            LaserSpawnPointsChanged?.Invoke(m_laserSpawnPoints);
        }
    }

    public float DamageRate
    {
        get => m_damageRate;
        set
        {
            m_damageRate = value;
            DamageRateChanged?.Invoke(m_damageRate);
        }
    }

    public float LaserLength
    {
        get => m_laserLength;
        set
        {
            m_laserLength = value;
            LaserLengthChanged?.Invoke(m_laserLength);
        }
    }

    public float LaserDamage
    {
        get => m_laserDamage;
        set
        {
            m_laserDamage = value;
            LaserDamageChanged?.Invoke(m_laserDamage);
        }
    }
}
