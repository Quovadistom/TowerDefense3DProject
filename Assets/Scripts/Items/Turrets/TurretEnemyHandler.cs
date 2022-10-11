using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class TurretEnemyHandler : MonoBehaviour
{
    private GenericRepository<BasicEnemy> m_enemiesInRange;

    public ITargetMethod TargetMethod { get; set; } = new TargetFirstEnemy();

    public BasicEnemy Target { get; private set; }

    private bool m_hasTarget;
    public bool HasTarget
    {
        get 
        { 
            return m_hasTarget; 
        }
        private set
        {
            if (value == m_hasTarget)
            {
                return;
            }

            m_hasTarget = value;

            if (!m_hasTarget)
            {
                OnTargetLost?.Invoke();
            }
        }
    }

    public event Action OnTargetLost;

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

    public bool IsTargetValid()
    {
        if (Target == null)
        {
            return false;
        }

        if (Target != null && (Target.IsPooled || !m_enemiesInRange.ReadOnlyList.Any()))
        {
            m_enemiesInRange.Remove(Target);
            Target = null;
            return false;
        }

        return true;
    }

    private void ScanForTarget()
    {
        if (IsTargetValid())
        {
            return;
        }

        if (!m_enemiesInRange.ReadOnlyList.Any())
        {
            HasTarget = false;
            return;
        }

        HasTarget = TargetMethod.TryGetTarget(m_enemiesInRange.ReadOnlyList, out BasicEnemy enemy);

        Target = enemy;
    }

    public class Factory : PlaceholderFactory<TurretEnemyHandler, TurretEnemyHandler>
    {
    }
}
