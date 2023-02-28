using UnityEngine;

public class Bullet : ProjectileBase
{
    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        enemy.TakeDamage(ProjectileProfile.Damage);

        //if (enemy.TryGetComponent(out StatusEffectContext statusEffectContext))
        //{
        //    statusEffectContext.ChangeStatusEffect(new FireStatusEffect(10, 0.5f, enemy));
        //}

        m_poolingService.ReturnPooledObject(this);
    }
}
