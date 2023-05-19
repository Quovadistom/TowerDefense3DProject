using UnityEngine;

public class ProjectileBase : AttackBase
{
    private Vector3 m_direction;
    private float m_distanceThisFrame;

    public override void Initialize(BasicEnemy target, ProjectileProfile bulletProfile, StatusEffect statusEffect)
    {
        base.Initialize(target, bulletProfile, statusEffect);
        m_direction = target.EnemyMiddle.transform.position - this.transform.position;
    }

    public override void Update()
    {
        base.Update();

        if (m_direction != default)
        {
            m_distanceThisFrame = ProjectileProfile.Speed * Time.deltaTime;
            transform.Translate(m_direction.normalized * m_distanceThisFrame, Space.World);
        }
    }
}