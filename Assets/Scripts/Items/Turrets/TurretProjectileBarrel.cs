using UnityEngine;
using Zenject;

public class TurretProjectileBarrel : TurretBarrel
{
    public ProjectileComponent m_projectileComponent;
    public FireRateComponent m_fireRateComponent;
    public DamageComponent m_damageComponent;
    public BulletBarrelComponent m_bulletBarrelComponent;

    private BulletService m_bulletService;

    public override float Interval => m_fireRateComponent.FireRate;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    public override void TimeElapsed(BasicEnemy basicEnemy)
    {
        foreach (Transform spawnPoint in m_bulletBarrelComponent.Visual.SpawnPoints)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile(m_projectileComponent.BulletSpeed, m_damageComponent.Damage);
            m_bulletService.CreateNewBullet(m_projectileComponent.BulletPrefab, spawnPoint.position, projectileProfile, basicEnemy, null);
        }
    }
}
