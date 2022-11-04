using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretBarrel : BaseVisualChanger<AttackMethodComponent>
{
    [SerializeField] private TurretTargetingComponent m_turretTargetingComponent;
    [SerializeField] private Transform m_barrelVisual;

    private Transform m_targetTransform;
    private BasicEnemy m_currentTarget;

    protected override void Awake()
    {
        base.Awake();

        OnTargetLost();
        m_turretTargetingComponent.TargetChanged += OnEnemyChanged;
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_turretTargetingComponent.TurnSpeed);

        LookAtTarget();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        m_turretTargetingComponent.TargetChanged -= OnEnemyChanged;
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
        Component.CurrentAttackMethod?.TargetLost();
    }

    private void LookAtTarget()
    {
        if (IsLockedOnTarget())
        {
            Component.CurrentAttackMethod?.Shoot(m_currentTarget);
        }
    }

    private bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_targetTransform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= 0.99;
    }
}
