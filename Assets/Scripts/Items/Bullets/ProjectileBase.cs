using UnityEngine;

public class ProjectileBase<T> : AttackBase<T> where T : IBulletProfile
{
    private Vector3 m_direction;
    private float m_distanceThisFrame;

    public override void Update()
    {
        base.Update();

        if (m_target != null)
        {
            m_direction = m_target.EnemyMiddle.transform.position - this.transform.position;
        }

        m_distanceThisFrame = BulletProfile.Speed * Time.deltaTime;
        transform.Translate(m_direction.normalized * m_distanceThisFrame, Space.World);
    }
}