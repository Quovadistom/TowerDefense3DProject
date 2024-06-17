using DG.Tweening;
using UnityEngine;

public abstract class TurretBarrel : ModuleWithModificationBase
{
    [SerializeField] private TowerModule m_towerInfoComponent;

    private float m_elapsedTime = 0;

    private BasicEnemy m_currentTarget = null;
    public BasicEnemy CurrentTarget => m_currentTarget;
    public float Accuracy = 0.99f;

    public TargetingModule TargetingModule;
    public FireRateModule FireRateModule;

    private bool m_updateAndFollowTarget = true;
    private Tweener m_towerRotationTween;

    public bool UpdateTarget
    {
        get => m_updateAndFollowTarget;
        set
        {
            if (value)
            {
                m_currentTarget = TargetingModule.Target.Value;
            }

            m_updateAndFollowTarget = value;
        }
    }

    protected virtual void Awake()
    {
        TargetingModule.Target.ValueChanged += RefreshTarget;
    }

    protected void OnDestroy()
    {
        TargetingModule.Target.ValueChanged -= RefreshTarget;
    }

    protected virtual void Update()
    {
        if (m_currentTarget != null)
        {
            m_towerRotationTween?.Kill();
            m_towerRotationTween = transform.DOLookAt(new(m_currentTarget.EnemyMiddle.position.x,
                transform.position.y,
                m_currentTarget.EnemyMiddle.position.z), 0.5f);
        }

        m_elapsedTime += Time.deltaTime;

        if (m_elapsedTime >= FireRateModule.FireRate.Value)
        {
            TimeElapsed(m_currentTarget);
            m_elapsedTime = 0;
        }
    }

    protected void ResetTimer() => m_elapsedTime = 0;

    private void RefreshTarget(BasicEnemy basicEnemy)
    {
        if (UpdateTarget)
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
