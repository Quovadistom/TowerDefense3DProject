using System;
using UnityEngine;
using Zenject;

public class TurretProjectileComponent : ChangeVisualComponent, ITowerComponent
{
    [SerializeField] private TurretTargetingComponent m_turretTargetingComponent;
    [SerializeField] private BulletSpawnPoints m_bulletSpawnPoints;
    [SerializeField] private ProjectileBase m_bulletPrefab;
    [SerializeField] private float m_fireRate;
    [SerializeField] private ProjectileProfile m_projectileProfile;

    public event Action<BulletSpawnPoints> BulletSpawnPointsChanged;
    public event Action<ProjectileBase> BulletPrefabChanged;
    public event Action<float> FirerateChanged;
    public event Action<ProjectileProfile> ProjectileProfileChanged;

    public BulletSpawnPoints BulletSpawnPoints
    {
        get => m_bulletSpawnPoints;
        set
        {
            Visual = value.transform;
            m_bulletSpawnPoints = Visual.GetComponent<BulletSpawnPoints>();
            BulletSpawnPointsChanged?.Invoke(m_bulletSpawnPoints);
        }
    }

    public ProjectileBase BulletPrefab
    {
        get => m_bulletPrefab;
        set
        {
            m_bulletPrefab = value;
            BulletPrefabChanged?.Invoke(m_bulletPrefab);
        }
    }

    public float Firerate
    {
        get => m_fireRate;
        set
        {
            m_fireRate = value;
            FirerateChanged?.Invoke(m_fireRate);
        }
    }

    public ProjectileProfile ProjectileProfile
    {
        get => m_projectileProfile;
        set
        {
            m_projectileProfile = value;
            ProjectileProfileChanged?.Invoke(m_projectileProfile);
        }
    }
}