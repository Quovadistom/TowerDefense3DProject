using System;
using UnityEngine;

public class TurretLaserComponent : ChangeVisualComponent
{
    [SerializeField] private TurretRangeComponent m_turretRangeComponent;
    [SerializeField] private LaserSpawnPoints m_laserSpawnPoints;

    [SerializeField] private float m_damageRate;
    [SerializeField] private float m_laserDuration;
    [SerializeField] private float m_laserCooldownDuration;
    [SerializeField] private float m_laserLength;
    [SerializeField] private float m_laserDamage;

    public event Action<LaserSpawnPoints> LaserSpawnPointsChanged;
    public event Action<float> DamageRateChanged;
    public event Action<float> LaserLengthChanged;
    public event Action<float> LaserDamageChanged;
    public event Action<float> LaserDurationChanged;
    public event Action<float> LaserCooldownDurationChanged;

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

    public float LaserDuration
    {
        get => m_laserDuration;
        set
        {
            m_laserDuration = value;
            LaserDurationChanged?.Invoke(m_laserDuration);
        }
    }

    public float LaserCooldownDuration
    {
        get => m_laserCooldownDuration;
        set
        {
            m_laserCooldownDuration = value;
            LaserCooldownDurationChanged?.Invoke(m_laserCooldownDuration);
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
