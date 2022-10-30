using NaughtyAttributes;
using System;
using UnityEngine;

public class BarrelTurretMediator : TurretMediatorBase
{
    private BasicEnemy m_currentTarget;
    private ITargetMethod m_currentTargetMethod;
    private IAttackMethod m_currentAttackMethod;

    [BoxGroup("Turret Settings")]
    [SerializeField] private float m_turnSpeed;
    [BoxGroup("Projectile Settings")]
    [SerializeField] private float m_damage;

    public event Action<BasicEnemy> TargetChanged;
    public event Action<ITargetMethod> TargetMethodChanged;
    public event Action<IAttackMethod> AttackMethodChanged;
    public event Action<float> TurnSpeedChanged;
    public event Action<float> DamageChanged;

    protected override void Start()
    {
        base.Start();

        CurrentTargetMethod = new TargetFirstEnemy();
    }

    public BasicEnemy CurrentTarget
    {
        get => m_currentTarget;
        set
        {
            m_currentTarget = value;
            TargetChanged?.Invoke(m_currentTarget);
        }
    }

    public ITargetMethod CurrentTargetMethod
    {
        get => m_currentTargetMethod;
        set
        {
            m_currentTargetMethod = value;
            TargetMethodChanged?.Invoke(m_currentTargetMethod);
        }
    }    
    
    public IAttackMethod CurrentAttackMethod
    {
        get => m_currentAttackMethod;
        set
        {
            m_currentAttackMethod = value;
            AttackMethodChanged?.Invoke(m_currentAttackMethod);
        }
    }

    public float TurnSpeed
    {
        get => m_turnSpeed;
        set
        {
            m_turnSpeed = value;
            TurnSpeedChanged?.Invoke(m_turnSpeed);
        }
    }

    public float Damage
    {
        get => m_damage;
        set
        {
            m_damage = value;
            DamageChanged?.Invoke(m_damage);
        }
    }
}