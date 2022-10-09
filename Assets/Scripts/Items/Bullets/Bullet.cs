using UnityEngine;

public class Bullet : ProjectileBase<IBulletProfile>
{
    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        enemy.TakeDamage(BulletProfile.Damage);
        m_poolingService.ReturnPooledObject(this);
    }
}
