using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ProjectileFiringMethod : IAttackMethod
{
    private BulletService m_bulletService;
    private TurretProjectileComponent m_turretProjectileComponent;
    private Timer m_timer;
    private bool m_timerElapsed = true;

    public ProjectileFiringMethod(BulletService bulletService, TurretProjectileComponent turretProjectileComponent, WaveService waveService)
    {
        m_bulletService = bulletService;
        m_turretProjectileComponent = turretProjectileComponent;

        m_turretProjectileComponent.FirerateChanged += OnFireRateChanged;

        m_timer = new Timer(m_turretProjectileComponent.Firerate * 1000);
        m_timer.Elapsed += OnTimerElapsed;
        waveService.WaveComplete += () => m_timer.Stop();
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

        foreach (Transform spawnPoint in m_turretProjectileComponent.BulletSpawnPoints.SpawnPoints)
        {
            m_bulletService.CreateNewBullet(m_turretProjectileComponent.BulletPrefab, spawnPoint.position, m_turretProjectileComponent.ProjectileProfile, target);
        }

        m_timer.Start();
    }

    public void TargetLost() { }
}