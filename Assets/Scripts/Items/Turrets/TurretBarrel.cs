using UnityEngine;

public abstract class TurretBarrel<T> : BaseVisualChanger<T> where T : ChangeVisualComponent
{
    [SerializeField] private TurretTargetingComponent m_turretTargetingComponent;

    private float m_elapsedTime = Mathf.Infinity;

    public BasicEnemy CurrentTarget => m_turretTargetingComponent.CurrentTarget;

    public abstract float Interval { get; }
    public float Accuracy = 0.99f;

    protected virtual void Update()
    {
        m_elapsedTime += Time.deltaTime;

        if (m_turretTargetingComponent.CurrentTarget == null)
        {
            return;
        }

        var lookPos = m_turretTargetingComponent.CurrentTarget.EnemyMiddle.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_turretTargetingComponent.TurnSpeed);

        if (m_elapsedTime > Interval)
        {
            if (m_turretTargetingComponent.CurrentTarget != null && IsLockedOnTarget())
            {
                m_elapsedTime = 0;
                DoDamage(m_turretTargetingComponent.CurrentTarget);
            }
        }
    }

    public abstract void DoDamage(BasicEnemy currentTarget);

    protected bool IsLockedOnTarget()
    {
        Vector3 dirFromAtoB = (m_turretTargetingComponent.CurrentTarget.EnemyMiddle.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
        return dotProd >= Accuracy;
    }
}
