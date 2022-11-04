using System;
using UnityEngine;
using Zenject;

public class TurretProjectileComponent : AttackMethodComponent
{
    [SerializeField] private BulletSpawnPoints m_bulletSpawnPoints;
    [SerializeField] private ProjectileBase<IBulletProfile> m_bulletPrefab;
    [SerializeField] private float m_fireRate;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private float m_bulletDamage;
    private StandardBulletProfile m_projectileProfile;

    public event Action<BulletSpawnPoints> BulletSpawnPointsChanged;
    public event Action<ProjectileBase<IBulletProfile>> BulletPrefabChanged;
    public event Action<float> FirerateChanged;
    public event Action<float> BulletSpeedChanged;
    public event Action<float> DamageChanged;
    public event Action<StandardBulletProfile> ProjectileProfileChanged;

    private BulletService m_bulletService;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    protected void Start()
    {
        ProjectileProfile = new StandardBulletProfile(BulletSpeed, BulletDamage);
        CurrentAttackMethod = new ProjectileFiringMethod<IBulletProfile>(m_bulletService, this);
    }

    public BulletSpawnPoints BulletSpawnPoints
    {
        get => m_bulletSpawnPoints;
        set
        {
            m_bulletSpawnPoints = Instantiate(value);
            Visual = m_bulletSpawnPoints.transform;
            BulletSpawnPointsChanged?.Invoke(m_bulletSpawnPoints);
        }
    }

    public ProjectileBase<IBulletProfile> BulletPrefab
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

    public float BulletSpeed
    {
        get => m_bulletSpeed;
        set
        {
            m_bulletSpeed = value;
            BulletSpeedChanged?.Invoke(m_bulletSpeed);
        }
    }

    public float BulletDamage
    {
        get => m_bulletDamage;
        set
        {
            m_bulletDamage = value;
            DamageChanged?.Invoke(m_bulletDamage);
        }
    }

    public StandardBulletProfile ProjectileProfile
    {
        get => m_projectileProfile;
        set
        {
            m_projectileProfile = value;
            ProjectileProfileChanged?.Invoke(m_projectileProfile);
        }
    }
}