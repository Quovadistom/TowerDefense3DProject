using UnityEngine;

public abstract class TurretBarrel : ModuleWithModificationBase
{
    [SerializeField] private TowerModule m_towerInfoComponent;

    private float m_elapsedTime = Mathf.Infinity;

    private BasicEnemy m_currentTarget = null;
    public BasicEnemy CurrentTarget => m_currentTarget;

    public abstract float Interval { get; }
    public float Accuracy = 0.99f;

    public TargetingModule TargetingComponent;

    private bool m_updateAndFollowTarget = true;
    public bool UpdateAndFollowTarget
    {
        get => m_updateAndFollowTarget;
        set
        {
            if (value)
            {
                m_currentTarget = TargetingComponent.Target.Value;
            }

            m_updateAndFollowTarget = value;
        }
    }

    protected virtual void Awake()
    {
        TargetingComponent.Target.ValueChanged += RefreshTarget;
    }

    protected void OnDestroy()
    {
        TargetingComponent.Target.ValueChanged -= RefreshTarget;
    }

    protected virtual void Update()
    {
        m_elapsedTime += Time.deltaTime;

        if (!m_towerInfoComponent.IsTowerPlaced ||
            m_currentTarget == null ||
            TargetingComponent.Target == null ||
            !UpdateAndFollowTarget)
        {
            return;
        }

        var lookPos = m_currentTarget.EnemyMiddle.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TargetingComponent.TurnSpeed.Value);

        if (m_elapsedTime > Interval)
        {
            if (IsLockedOnTarget(m_currentTarget))
            {
                m_elapsedTime = 0;
                TimeElapsed(m_currentTarget);
            }
        }
    }

    private void RefreshTarget(BasicEnemy basicEnemy)
    {
        if (UpdateAndFollowTarget)
        {
            m_currentTarget = basicEnemy;
        }
    }

    public abstract void TimeElapsed(BasicEnemy currentTarget);

    protected bool IsLockedOnTarget(BasicEnemy target)
    {
        Vector3 dirFromAtoB = (target.EnemyMiddle.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= Accuracy;
    }
}
