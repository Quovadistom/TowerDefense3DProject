using UnityEngine;

public class Bullet : ProjectileBase
{
    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        enemy.TakeDamage(ProjectileProfile.Damage);
        m_poolingService.ReturnPooledObject(this);
    }
}
