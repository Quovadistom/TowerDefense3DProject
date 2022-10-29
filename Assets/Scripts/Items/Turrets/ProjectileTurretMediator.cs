using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class ProjectileTurretMediator : BarrelTurretMediator
{
    [Header("Projectile Settings")]
    public BulletSpawnPoints ProjectileSpawnPoints;
    public ProjectileBase<IBulletProfile> BulletPrefab;
    public float BulletSpeed = 20;

    private BulletService m_bulletService;

    public StandardBulletProfile ProjectileProfile { get; private set; }

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    protected override void Start()
    {
        base.Start();

        ProjectileProfile = new StandardBulletProfile(BulletSpeed, Damage);
        CurrentAttackMethod = new ProjectileFiringMethod<IBulletProfile>(m_bulletService, this);
    }
}