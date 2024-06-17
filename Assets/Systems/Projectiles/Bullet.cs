public class Bullet : ProjectileBase
{
    protected override void OnCollisionWithEnemy(BasicEnemy enemy)
    {
        enemy.TakeDamage(ProjectileProfile.Damage);

        if (enemy.TryGetComponent(out StatusEffectHandler statusEffectContext))
        {
            statusEffectContext.RequestChangeStatusEffect(StatusEffect);
        }

        m_poolingService.ReturnPooledObject(this);
    }
}
