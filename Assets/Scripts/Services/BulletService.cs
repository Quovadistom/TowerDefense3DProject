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

    public ProjectileBase<T> CreateNewBullet<T>(ProjectileBase<T> bulletPrefab, Vector3 position, T bulletProfile, BasicEnemy enemy) where T : IBulletProfile
    {
        ProjectileBase<T> newBullet = (ProjectileBase<T>) m_poolingService.GetPooledObject(bulletPrefab);
        newBullet.SetProfile(bulletProfile);
        newBullet.transform.position = position;
        newBullet.SetAndSeekEnemy(enemy);
        return newBullet;
    }
}
