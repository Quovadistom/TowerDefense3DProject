using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class ProjectileTurretData : TurretMediator
{
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

    protected override void Awake()
    {
        base.Awake();

        ProjectileProfile = new StandardBulletProfile(BulletSpeed, Damage);
        FiringMethod = new ProjectileFiringMethod<IBulletProfile>(m_bulletService, this);
    }
}