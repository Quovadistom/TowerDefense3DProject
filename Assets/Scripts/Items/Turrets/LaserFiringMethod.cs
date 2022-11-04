using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class LaserFiringMethod : IAttackMethod
{
    private LayerSettings m_layerSettings;
    private TurretLaserComponent m_laserComponent;
    private TurretRangeComponent m_turretRangeComponent;
    private Timer m_timer;
    private bool m_timerElapsed = true;

    public LaserFiringMethod(LayerSettings layerSettings, TurretLaserComponent laserTurretData, TurretRangeComponent turretRangeComponent)
    {
        m_layerSettings = layerSettings;
        m_laserComponent = laserTurretData;
        m_turretRangeComponent = turretRangeComponent;

        m_timer = new Timer(m_laserComponent.DamageRate * 1000);
        m_timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        m_timerElapsed = true;
    }

    public void Shoot(BasicEnemy target)
    {
        foreach (LineRenderer lineRenderer in m_laserComponent.LaserSpawnPoints.SpawnPoints)
        {
            lineRenderer.enabled = true;
            Vector3 lineRendererTransformPosition = lineRenderer.transform.position;
            Vector3 targetPosition = target.EnemyMiddle.transform.position - lineRendererTransformPosition;

            RaycastHit[] enemies = Physics.RaycastAll(lineRendererTransformPosition,
                targetPosition,
                m_turretRangeComponent.Range,
                m_layerSettings.EnemyLayer);

            lineRenderer.useWorldSpace = true;

            if (enemies.Length > 0)
            {
                lineRenderer.enabled = true;
                Vector3[] positions = new Vector3[]
                {
                    lineRendererTransformPosition,
                    m_laserComponent.LaserLength * Vector3.Normalize(targetPosition) + lineRendererTransformPosition
                };

                lineRenderer.SetPositions(positions);

                DamageEnemies(enemies);
            }
        }
    }

    private void DamageEnemies(RaycastHit[] enemies)
    {
        if (!m_timerElapsed)
        {
            return;
        }

        foreach (RaycastHit enemy in enemies)
        {
            if (enemy.rigidbody.TryGetComponent(out BasicEnemy basicEnemy))
            {
                basicEnemy.TakeDamage(m_laserComponent.LaserDamage);
            }
        }

        m_timer.Start();
        m_timerElapsed = false;
    }

    public void TargetLost()
    {
        foreach (LineRenderer lineRenderer in m_laserComponent.LaserSpawnPoints.SpawnPoints)
        {
            lineRenderer.enabled = false;
        }
    }
}