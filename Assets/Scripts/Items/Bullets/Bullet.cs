public class Bullet : ProjectileBase
{
    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        enemy.TakeDamage(ProjectileProfile.Damage);

        if (enemy.TryGetComponent(out StatusEffectHandler statusEffectContext))
        {
            statusEffectContext.RequestChangeStatusEffect(new FireStatusEffect(10, 0.5f, 2));
        }

        m_poolingService.ReturnPooledObject(this);
    }
}
