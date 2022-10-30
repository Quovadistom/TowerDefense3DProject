using NaughtyAttributes;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class ProjectileTurretMediator : BarrelTurretMediator
{
    [BoxGroup("UI References")]
    [SerializeField] private BulletSpawnPoints m_projectileSpawnPoints;
    [BoxGroup("UI References")]
    [SerializeField] private ProjectileBase<IBulletProfile> m_bulletPrefab;
    [BoxGroup("Barrel Settings")]
    [SerializeField] private float m_fireRate;
    [BoxGroup("Projectile Settings")]
    [SerializeField] private float m_bulletSpeed = 20;
    private StandardBulletProfile m_projectileProfile;

    public event Action<BulletSpawnPoints> ProjectileSpawnPointsChanged;
    public event Action<ProjectileBase<IBulletProfile>> BulletPrefabChanged;
    public event Action<float> FirerateChanged;
    public event Action<float> BulletSpeedChanged;
    public event Action<StandardBulletProfile> ProjectileProfileChanged;

    private BulletService m_bulletService;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    protected override void Start()
    {
        base.Start();

        ProjectileProfile = new StandardBulletProfile(m_bulletSpeed, Damage);
        CurrentAttackMethod = new ProjectileFiringMethod<IBulletProfile>(m_bulletService, this);
    }

    public BulletSpawnPoints ProjectileSpawnPoints
    {
        get => m_projectileSpawnPoints;
        set
        {
            m_projectileSpawnPoints = Instantiate(value);
            ProjectileSpawnPointsChanged?.Invoke(m_projectileSpawnPoints);
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
            Debug.Log(value);
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