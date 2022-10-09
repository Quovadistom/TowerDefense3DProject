using UnityEngine;
using Zenject;

public class TurretBase : MonoBehaviour
{
    public BasicTurretData TurretData;

    private GenericRepository<BasicEnemy> m_enemiesInRange;

    public ITargetMethod TargetMethod { get; set; } = new TargetFirstEnemy();

    public BasicEnemy Target { get; private set; }

    private void Awake()
    {
        m_enemiesInRange = new GenericRepository<BasicEnemy>();
        InvokeRepeating(nameof(ScanForTarget), 0.2f, 0.2f);
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
        if (Target == null || (Target != null && Target.IsPooled))
        {
            m_enemiesInRange.Remove(Target);
            Target = null;
            return false;
        }

        return true;
    }

    private void ScanForTarget()
    {
        TargetMethod.TryGetTarget(m_enemiesInRange.ReadOnlyList, out BasicEnemy enemy);
        Target = enemy;
    }

    public class Factory : PlaceholderFactory<TurretBase, TurretBase>
    {
    }
}
