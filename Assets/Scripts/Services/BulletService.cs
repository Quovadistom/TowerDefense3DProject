using UnityEngine;

public class BulletService
{
    public WaypointCollection WaypointsCollection;
    private PoolingService m_poolingService;

    public Bullets Bullets { get; private set; }

    public BulletService(PoolingService poolingService)
    {
        m_poolingService = poolingService;

        Bullets = new Bullets();
    }

    public ProjectileBase CreateNewBullet(ProjectileBase bulletPrefab, Vector3 position, ProjectileProfile bulletProfile, BasicEnemy enemy, StatusEffect statusEffect)
    {
        ProjectileBase newBullet = (ProjectileBase)m_poolingService.GetPooledObject(bulletPrefab);
        newBullet.Initialize(enemy, bulletProfile, statusEffect);
        newBullet.transform.position = position;
        return newBullet;
    }
}
