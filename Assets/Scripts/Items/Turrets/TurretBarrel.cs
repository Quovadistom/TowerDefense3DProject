using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretBarrel : MonoBehaviour
{
    [SerializeField] private TurretMediator m_turretMediator;

    private Transform m_targetTransform;
    private BasicEnemy m_currentTarget;

    private void Awake()
    {
        OnTargetLost();
        m_turretMediator.OnEnemyChanged += OnEnemyChanged;
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
        m_turretMediator.OnEnemyChanged -= OnEnemyChanged;
    }

    private void OnEnemyChanged(BasicEnemy newEnemy)
    {
        m_currentTarget = newEnemy;

        if (newEnemy == null)
        {
            OnTargetLost();
        }
    }

    private void OnTargetLost()
    {
        m_turretMediator.FiringMethod.TargetLost();
    }

    private void LookAtTarget()
    {
        if (IsLockedOnTarget())
        {
            m_turretMediator.FiringMethod.Shoot(m_currentTarget);
        }
    }

    private bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_targetTransform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= 0.99;
    }
}
