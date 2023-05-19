using UnityEngine;
using Zenject;

public class TurretProjectileBarrel : TurretBarrel<TurretProjectileComponent>
{
    private BulletService m_bulletService;

    public override float Interval => Component.Firerate;

    [Inject]
    public void Construct(BulletService bulletService)
    {
        m_bulletService = bulletService;
    }

    public override void TimeElapsed(BasicEnemy basicEnemy)
    {
        foreach (Transform spawnPoint in Component.BulletSpawnPoints.SpawnPoints)
        {
            m_bulletService.CreateNewBullet(Component.BulletPrefab, spawnPoint.position, Component.ProjectileProfile, basicEnemy, TurretStatusEffectComponent.StatusEffect);
        }
    }
}
