using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretEnemyHandler : ModuleWithModificationBase
{
    [SerializeField] private TowerModule m_towerModule;

    public TargetMethodModule TargetMethodModule = new();

    private List<BasicEnemy> m_enemiesInRange = new();

    private BasicEnemy m_currentTarget = null;
    public BasicEnemy CurrentTarget
    {
        get => m_currentTarget;
        set
        {
            if (m_currentTarget == value)
            {
                return;
            }

            m_currentTarget = value;
            m_towerModule.TryFindAndActOnModule<TargetingModule>((component) => component.Target.Value = value);
        }
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
        if (CurrentTarget != null && (CurrentTarget.IsPooled || !m_enemiesInRange.Any()))
        {
            m_enemiesInRange.Remove(CurrentTarget);
            CurrentTarget = null;
        }
    }

    private void ScanForTarget()
    {
        CheckTargetValidity();

        if (!m_enemiesInRange.Any())
        {
            return;
        }

        TargetMethodModule.TargetMethod.TryGetTarget(this, m_enemiesInRange, out BasicEnemy enemy);
        CurrentTarget = enemy;
    }
}
