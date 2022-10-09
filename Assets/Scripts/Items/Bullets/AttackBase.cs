using UnityEngine;

public class AttackBase<T> : Poolable where T : IAttackProfileBase
{
    protected BasicEnemy m_target;

    public T BulletProfile { get; private set; }

    public virtual void Update()
    {
        if (m_target != null && m_target.IsPooled)
        {
            m_target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy))
        {
            OnCollisionWithEnemy(enemy);
        }
    }

    protected virtual void OnCollisionWithEnemy(BasicEnemy enemy)
    {
    }

    public void SetAndSeekEnemy(BasicEnemy target)
    {
        m_target = target;
    }

    internal void SetProfile(T bulletProfile)
    {
        BulletProfile = bulletProfile;
    }
}
