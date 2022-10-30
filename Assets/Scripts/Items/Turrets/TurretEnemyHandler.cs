using System.Linq;
using UnityEngine;
using Zenject;

public class TurretEnemyHandler : MonoBehaviour
{
    [SerializeField] private BarrelTurretMediator m_turretMediator; 

    private GenericRepository<BasicEnemy> m_enemiesInRange;
    private BasicEnemy m_target;

    private void Awake()
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
        BasicEnemy enemy = other.attachedRigidbody.GetComponent<BasicEnemy>();
        m_enemiesInRange.Remove(enemy);
    }

    public void CheckTargetValidity()
    {
        if (m_target != null && (m_target.IsPooled || !m_enemiesInRange.ReadOnlyList.Any()))
        {
            m_enemiesInRange.Remove(m_target);
            m_target = null;
            m_turretMediator.CurrentTarget = m_target;
        }
    }

    private void ScanForTarget()
    {
        CheckTargetValidity();

        if (!m_enemiesInRange.ReadOnlyList.Any())
        {
            return;
        }

        m_turretMediator.CurrentTargetMethod.TryGetTarget(m_enemiesInRange.ReadOnlyList, out BasicEnemy enemy);

        m_turretMediator.CurrentTarget = enemy;
        m_target = enemy;
    }

    public class Factory : PlaceholderFactory<TurretEnemyHandler, TurretEnemyHandler>
    {
    }
}
