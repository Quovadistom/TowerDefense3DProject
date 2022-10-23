using System;
using UnityEngine;

public class BarrelTurretMediator : TurretMediatorBase
{
    public float Firerate = 1;
    public float Damage = 50;
    public float TurnSpeed = 8;

    public IAttackMethod FiringMethod { get; protected set; }
    public ITargetMethod TargetMethod { get; protected set; }

    private BasicEnemy m_currentTarget;
    public event Action<BasicEnemy> OnEnemyChanged;
    public event Action<ITargetMethod> OnTargetMethodChanged;

    protected virtual void Awake()
    {
        SetTargetMethod(new TargetFirstEnemy());
    }

    public void SetTarget(BasicEnemy newEnemy)
    {
        if (newEnemy == m_currentTarget)
        {
            return;
        }

        m_currentTarget = newEnemy;
        OnEnemyChanged?.Invoke(m_currentTarget);
    }

    public void SetTargetMethod(ITargetMethod method)
    {
        TargetMethod = method;
        OnTargetMethodChanged?.Invoke(TargetMethod);
    }

    public void SetBarrel(float? fireRate = null, float? damage = null, float? turnSpeed = null)
    {
        Firerate = fireRate != null ? (float)fireRate : Firerate;
        Damage = damage != null ? (float)damage : Damage;
        TurnSpeed = turnSpeed != null ? (float)turnSpeed : TurnSpeed;
    }
}