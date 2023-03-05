public class FireStatusEffect : StatusEffect
{
    public override EffectType EffectTypeType => EffectType.FIRE;

    private float m_damageAmount;

    public FireStatusEffect(float damageAmount, float damageRate, float effectTime) : base(damageRate, effectTime)
    {
        m_damageAmount = damageAmount;
    }

    public override void ApplyEffect(BasicEnemy basicEnemy)
    {
        basicEnemy.TakeDamage(m_damageAmount);
    }

    public override void RequestEffectChange(StatusEffect newStatusEffect)
    {
        //if (newStatusEffect is CorrosionStatusEffect)
        //{

        //}
    }
}
