using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TurretLaserBarrel : TurretBarrel<TurretLaserComponent>
{
    private bool m_isLaserEnabled = true;
    private LayerSettings m_layerSettings;
    private List<RaycastHit>[] m_enemies;

    private float m_activeLaserDuration = 0;
    private float m_cooldownDuration = 0;

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

        if (!m_isLaserEnabled)
        {
            m_cooldownDuration += Time.deltaTime;

            if (m_cooldownDuration <= Component.LaserCooldownDuration)
            {
                return;
            }
        }

        if (CurrentTarget == null)
        {
            SetLaserState(false);
            return;
        }

        m_activeLaserDuration += Time.deltaTime;

        if (m_activeLaserDuration <= Component.LaserDuration)
        {
            SetLaserState(true);
            CreateLaser();
            return;
        }

        m_cooldownDuration = 0;
        m_activeLaserDuration = 0;
        SetLaserState(false);
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

    public override void TimeElapsed(BasicEnemy target)
    {
        if (!m_isLaserEnabled)
        {
            return;
        }

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
