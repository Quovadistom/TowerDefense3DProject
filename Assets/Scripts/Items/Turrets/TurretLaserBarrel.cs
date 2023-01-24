using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public class TurretLaserBarrel : TurretBarrel<TurretLaserComponent>
{
    private bool m_isLaserEnabled = true;
    private LayerSettings m_layerSettings;
    private List<RaycastHit>[] m_enemies;

    public override float Interval => Component.DamageRate;

    [Inject]
    public void Construct(LayerSettings layerSettings)
    {
        m_layerSettings = layerSettings;
    }

    protected override void Awake()
    {
        base.Awake();

        OnLaserSpawnPointsChanged(Component.LaserSpawnPoints);
        OnLaserLengthChanged(Component.LaserLength);

        Component.LaserSpawnPointsChanged += OnLaserSpawnPointsChanged;
        Component.LaserLengthChanged += OnLaserLengthChanged;
    }

    protected override void Update()
    {
        base.Update();

        SetLaserState(CurrentTarget != null);

        if (m_isLaserEnabled)
        {
            CreateLaser();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Component.LaserSpawnPointsChanged -= OnLaserSpawnPointsChanged;
        Component.LaserLengthChanged -= OnLaserLengthChanged;

    }

    private void OnLaserSpawnPointsChanged(LaserSpawnPoints laserSpawnPoints)
    {
        m_enemies = new List<RaycastHit>[laserSpawnPoints.SpawnPoints.Count];
    }

    private void OnLaserLengthChanged(float length)
    {
        foreach (LineRenderer lineRenderer in Component.LaserSpawnPoints.SpawnPoints)
        {
            Vector3[] positions = new Vector3[]
            {
                    Vector3.zero,
                    new Vector3(0, 0, Component.LaserLength)
            };

            lineRenderer.SetPositions(positions);
        }
    }

    private void CreateLaser()
    {
        for (int i = 0; i < Component.LaserSpawnPoints.SpawnPoints.Count; i++)
        {
            LineRenderer lineRenderer = Component.LaserSpawnPoints.SpawnPoints[i];

            m_enemies[i] = Physics.RaycastAll(lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(0)),
                    lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(1)) - lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(0)),
                    100000,
                    m_layerSettings.EnemyLayer).ToList();
        }
    }

    private void SetLaserState(bool state)
    {
        if (m_isLaserEnabled != state)
        {
            foreach (LineRenderer lineRenderer in Component.LaserSpawnPoints.SpawnPoints)
            {
                lineRenderer.enabled = state;
            }

            m_isLaserEnabled = state;
        }
    }

    public override void DoDamage(BasicEnemy target)
    {
        for (int i = 0; i < m_enemies.Length; i++)
        {
            if (m_enemies[i] == null)
            {
                continue;
            }

            foreach (RaycastHit enemy in m_enemies[i])
            {
                if (enemy.rigidbody == null)
                {
                    return;
                }

                if (enemy.rigidbody.TryGetComponent(out BasicEnemy basicEnemy))
                {
                    basicEnemy.TakeDamage(Component.LaserDamage);
                }
            }
        }
    }
}
