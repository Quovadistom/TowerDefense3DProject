using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Zenject;

public class ProjectileFiringMethod : IAttackMethod
{
    private BasicTurretData m_turretData;
    private Timer m_timer;
    private bool m_timerElapsed = true;

    public ProjectileFiringMethod(BasicTurretData basicTurretData)
    {
        m_turretData = basicTurretData;

        m_timer = new Timer(m_turretData.Firerate * 1000);
        m_timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        m_timerElapsed = true;
    }

    public void Shoot(BulletService bulletService, IReadOnlyList<Transform> bulletSpawnPointsList, BasicEnemy target)
    {
        if (!m_timerElapsed)
        {
            return;
        }

        m_timerElapsed = false;

        foreach (Transform spawnPoint in bulletSpawnPointsList)
        {
            bulletService.CreateNewBullet(m_turretData.BulletPrefab, spawnPoint.position, m_turretData.AttackProfile, target);
        }

        m_timer.Start();
    }
}