using System;
using UnityEngine;

public class TurretTargetingComponent : MonoBehaviour
{
    private BasicEnemy m_currentTarget;
    private ITargetMethod m_currentTargetMethod;

    [SerializeField] private float m_turnSpeed;

    public event Action<BasicEnemy> TargetChanged;
    public event Action<ITargetMethod> TargetMethodChanged;
    public event Action<float> TurnSpeedChanged;

    protected void Start()
    {
        CurrentTargetMethod ??= new TargetFirstEnemy();
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

    public float TurnSpeed
    {
        get => m_turnSpeed;
        set
        {
            m_turnSpeed = value;
            TurnSpeedChanged?.Invoke(m_turnSpeed);
        }
    }
}