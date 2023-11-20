using UnityEngine;
using Zenject;

public class TurretProjectileBarrel : TurretBarrel
{
    public ProjectileModule m_projectileModule;
    public FireRateModule m_fireRateModule;
    public DamageModule m_damageModule;
    public TurretStatusEffectModule m_turretStatusEffectModule = new();
    public ModuleDataTypeVisual<BulletSpawnPoints> m_bulletBarrelModule;

    private BulletService m_bulletService;

    public override float Interval => m_fireRateModule.FireRate.Value;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    public override void TimeElapsed(BasicEnemy basicEnemy)
    {
        foreach (Transform spawnPoint in m_bulletBarrelModule.Visual.SpawnPoints)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile(m_projectileModule.BulletSpeed.Value, m_damageModule.Damage.Value);
            m_bulletService.CreateNewBullet(m_projectileModule.BulletPrefab.Value, spawnPoint.position, projectileProfile, basicEnemy, null);
        }
    }
}
