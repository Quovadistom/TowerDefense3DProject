using System;
using UnityEngine;
using Zenject;

[Serializable]
public class ProjectileTurretData : TurretAndAttackData<BulletSpawnPoints>
{
    public ProjectileBase<IBulletProfile> BulletPrefab;
    public float BulletSpeed = 20;
    private BulletService m_bulletService;

    public StandardBulletProfile ProjectileProfile { get; private set; }

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    private void Awake()
    {
        ProjectileProfile = new StandardBulletProfile(BulletSpeed, Damage);
        FiringMethod = new ProjectileFiringMethod<IBulletProfile>(m_bulletService, this);
    }
}