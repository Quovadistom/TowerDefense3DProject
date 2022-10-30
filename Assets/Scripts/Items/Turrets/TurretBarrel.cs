using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretBarrel : MonoBehaviour
{
    [SerializeField] private ProjectileTurretMediator m_turretMediator;
    [SerializeField] private BulletSpawnPoints m_bulletSpawnPoints;

    private Transform m_targetTransform;
    private BasicEnemy m_currentTarget;

    private void Awake()
    {
        OnTargetLost();
        m_turretMediator.TargetChanged += OnEnemyChanged;
        m_turretMediator.ProjectileSpawnPointsChanged += OnProjectileSpawnPointChanged;
    }

    private void Update()
    {
        if (m_currentTarget == null)
        {
            return;
        }

        m_targetTransform = m_currentTarget.EnemyMiddle;
        var lookPos = m_targetTransform.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_turretMediator.TurnSpeed);

        LookAtTarget();
    }

    private void OnDestroy()
    {
        m_turretMediator.TargetChanged -= OnEnemyChanged;
        m_turretMediator.ProjectileSpawnPointsChanged -= OnProjectileSpawnPointChanged;
    }

    private void OnEnemyChanged(BasicEnemy newEnemy)
    {
        m_currentTarget = newEnemy;

        if (newEnemy == null)
        {
            OnTargetLost();
        }
    }

    private void OnProjectileSpawnPointChanged(BulletSpawnPoints bulletSpawnPoints)
    {
        Destroy(m_bulletSpawnPoints.gameObject);
        bulletSpawnPoints.transform.SetParent(this.transform, false);
        m_bulletSpawnPoints = bulletSpawnPoints;
    }

    private void OnTargetLost()
    {
        m_turretMediator.CurrentAttackMethod?.TargetLost();
    }

    private void LookAtTarget()
    {
        if (IsLockedOnTarget())
        {
            m_turretMediator.CurrentAttackMethod?.Shoot(m_currentTarget);
        }
    }

    private bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_targetTransform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= 0.99;
    }
}
