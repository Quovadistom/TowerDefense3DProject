using System.Linq;
using UnityEngine;

public class TurretEnemyHandler : ModuleWithModificationBase
{
    [SerializeField] private TowerModule m_towerModule;

    public TargetMethodModule TargetMethodModule = new();

    private GenericRepository<BasicEnemy> m_enemiesInRange;

    private BasicEnemy m_currentTarget = null;
    public BasicEnemy CurrentTarget
    {
        get => m_currentTarget;
        set
        {
            m_currentTarget = value;
            m_towerModule.TryFindAndActOnComponent<TargetingModule>((component) => component.Target.Value = value);
        }
    }

    protected void Awake()
    {
        m_enemiesInRange = new GenericRepository<BasicEnemy>();
    }

    private void Update()
    {
        ScanForTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy))
        {
            m_enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy))
        {
            m_enemiesInRange.Remove(enemy);
        }
    }

    public void CheckTargetValidity()
    {
        if (CurrentTarget != null && (CurrentTarget.IsPooled || !m_enemiesInRange.ReadOnlyList.Any()))
        {
            m_enemiesInRange.Remove(CurrentTarget);
            CurrentTarget = null;
        }
    }

    private void ScanForTarget()
    {
        CheckTargetValidity();

        if (!m_enemiesInRange.ReadOnlyList.Any())
        {
            return;
        }

        TargetMethodModule.TargetMethod.TryGetTarget(this, m_enemiesInRange.ReadOnlyList, out BasicEnemy enemy);
        CurrentTarget = enemy;
    }
}
