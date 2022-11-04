using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ProjectileFiringMethod<T> : IAttackMethod where T : IBulletProfile
{
    private BulletService m_bulletService;
    private TurretProjectileComponent m_projectileTurretData;
    private Timer m_timer;
    private bool m_timerElapsed = true;

    public ProjectileFiringMethod(BulletService bulletService, TurretProjectileComponent projectileTurretData)
    {
        m_bulletService = bulletService;
        m_projectileTurretData = projectileTurretData;

        m_projectileTurretData.FirerateChanged += OnFireRateChanged;

        m_timer = new Timer(m_projectileTurretData.Firerate * 1000);
        m_timer.Elapsed += OnTimerElapsed;
    }

    private void OnFireRateChanged(float newFireRate)
    {
        m_timer.Interval = newFireRate * 1000;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        m_timerElapsed = true;
    }

    public void Shoot(BasicEnemy target)
    {
        if (!m_timerElapsed)
        {
            return;
        }

        m_timerElapsed = false;

        foreach (Transform spawnPoint in m_projectileTurretData.BulletSpawnPoints.SpawnPoints)
        {
            m_bulletService.CreateNewBullet(m_projectileTurretData.BulletPrefab, spawnPoint.position, m_projectileTurretData.ProjectileProfile, target);
        }

        m_timer.Start();
    }

    public void TargetLost() { }
}