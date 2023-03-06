using UnityEngine;

public abstract class TurretBarrel<T> : BaseVisualChanger<T> where T : ChangeVisualComponent
{
    [SerializeField] private TurretTargetingComponent m_turretTargetingComponent;
    [SerializeField] private TowerInfoComponent m_towerInfoComponent;

    private float m_elapsedTime = Mathf.Infinity;

    private BasicEnemy m_currentTarget = null;
    public BasicEnemy CurrentTarget => m_currentTarget;

    public abstract float Interval { get; }
    public float Accuracy = 0.99f;

    protected virtual void Update()
    {
        m_elapsedTime += Time.deltaTime;

        if (!m_towerInfoComponent.IsTowerPlaced)
        {
            return;
        }

        if (m_currentTarget == null || m_turretTargetingComponent.CurrentTarget == null)
        {
            RefreshTarget();
            return;
        }

        var lookPos = m_currentTarget.EnemyMiddle.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_turretTargetingComponent.TurnSpeed);

        if (m_elapsedTime > Interval)
        {
            if (IsLockedOnTarget(m_currentTarget))
            {
                m_elapsedTime = 0;
                TimeElapsed(m_currentTarget);
                RefreshTarget();
            }
        }
    }

    private void RefreshTarget()
    {
        m_currentTarget = m_turretTargetingComponent.CurrentTarget;
    }

    public abstract void TimeElapsed(BasicEnemy currentTarget);

    protected bool IsLockedOnTarget(BasicEnemy target)
    {
        Vector3 dirFromAtoB = (target.EnemyMiddle.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= Accuracy;
    }
}
