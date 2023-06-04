using System;
using UnityEngine;

[Serializable]
public class TargetingComponent : ComponentBase
{
    private BasicEnemy m_target;
    public Action<BasicEnemy> TargetChanged;

    public BasicEnemy Target
    {
        get { return m_target; }
        set
        {
            m_target = value;
            TargetChanged?.Invoke(value);
        }
    }

    [SerializeField] private float m_turnSpeed;

    public float TurnSpeed
    {
        get { return m_turnSpeed; }
        set { m_turnSpeed = value; }
    }
}