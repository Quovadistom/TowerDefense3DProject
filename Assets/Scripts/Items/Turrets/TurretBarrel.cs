using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretBarrel : Swappable<BulletSpawnPoints>
{
    [SerializeField] private TurretEnemyHandler m_turret;
    [SerializeField] private TurretData m_turretData;

    private Transform m_targetTransform;

    private void Awake()
    {
        OnTargetLost();
        m_turret.OnTargetLost += OnTargetLost;
    }

    private void Update()
    {
        if (m_turret.IsTargetValid())
        {
            m_targetTransform = m_turret.Target.EnemyMiddle;
            var lookPos = m_targetTransform.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_turretData.TurnSpeed);

            LookAtTarget();
        }
    }

    private void OnDestroy()
    {
        m_turret.OnTargetLost -= OnTargetLost;
    }

    private void OnTargetLost()
    {
        m_turretData.FiringMethod.TargetLost();
    }

    private void LookAtTarget()
    {
        if (IsLockedOnTarget())
        {
            m_turretData.FiringMethod.Shoot(m_turret.Target);
        }
    }

    private bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_targetTransform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= 0.99;
    }
}
