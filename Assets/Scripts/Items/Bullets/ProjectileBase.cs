using UnityEngine;

public class ProjectileBase : AttackBase
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

        m_distanceThisFrame = ProjectileProfile.Speed * Time.deltaTime;
        transform.Translate(m_direction.normalized * m_distanceThisFrame, Space.World);
    }
}