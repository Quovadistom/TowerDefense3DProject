using System;
using System.Linq;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

public class TurretEnemyHandler : MonoBehaviour
{
    [SerializeField] private BarrelTurretMediator m_turretMediator; 

    private GenericRepository<BasicEnemy> m_enemiesInRange;
    private ITargetMethod m_targetMethod;

    public BasicEnemy Target { get; private set; }

    private void Awake()
    {
        m_turretMediator.OnTargetMethodChanged += TargetMethodChanged;
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
        BasicEnemy enemy = other.attachedRigidbody.GetComponent<BasicEnemy>();
        m_enemiesInRange.Remove(enemy);
    }

    private void OnDestroy()
    {
        m_turretMediator.OnTargetMethodChanged -= TargetMethodChanged;
    }

    private void TargetMethodChanged(ITargetMethod targetMethod) => m_targetMethod = targetMethod;

    public void CheckTargetValidity()
    {
        if (Target != null && (Target.IsPooled || !m_enemiesInRange.ReadOnlyList.Any()))
        {
            m_enemiesInRange.Remove(Target);
            Target = null;
            m_turretMediator.SetTarget(Target);
        }
    }

    private void ScanForTarget()
    {
        CheckTargetValidity();

        if (!m_enemiesInRange.ReadOnlyList.Any())
        {
            return;
        }

        m_targetMethod.TryGetTarget(m_enemiesInRange.ReadOnlyList, out BasicEnemy enemy);

        m_turretMediator.SetTarget(enemy);
        Target = enemy;
    }

    public class Factory : PlaceholderFactory<TurretEnemyHandler, TurretEnemyHandler>
    {
    }
}
