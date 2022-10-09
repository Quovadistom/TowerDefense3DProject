using UnityEngine;

public class ProjectileBase<T> : Poolable where T : IBulletProfile
{
    private Vector3 m_direction;
    private float m_distanceThisFrame;
    protected BasicEnemy m_target;

    public T BulletProfile { get; private set; }

    private void Update()
    {
        if (m_target != null && m_target.IsPooled)
        {
            m_target = null;
        }

        if (m_target != null)
        {
            m_direction = m_target.transform.position - this.transform.position;
        }

        m_distanceThisFrame = BulletProfile.Speed * Time.deltaTime;
        transform.Translate(m_direction.normalized * m_distanceThisFrame, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out BasicEnemy enemy))
        {
            OnCollisionWithEnemy(enemy);
            m_poolingService.ReturnPooledObject(this);
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
