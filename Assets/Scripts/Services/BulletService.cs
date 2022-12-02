using System.Collections;
using System.Collections.Generic;
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

    public ProjectileBase CreateNewBullet(ProjectileBase bulletPrefab, Vector3 position, ProjectileProfile bulletProfile, BasicEnemy enemy)
    {
        ProjectileBase newBullet = (ProjectileBase) m_poolingService.GetPooledObject(bulletPrefab);
        newBullet.SetProfile(bulletProfile);
        newBullet.transform.position = position;
        newBullet.SetEnemy(enemy);
        return newBullet;
    }
}
