public class FireStatusEffect : StatusEffect
{
    private float m_damageAmount;

    public FireStatusEffect(StatusContext statusContext, float damageRate, float damageAmount) : base(statusContext, damageRate) 
    {
        m_damageAmount = damageAmount;
    }

    public override void ApplyEffect()
    {
        StatusContext.Enemy.TakeDamage(m_damageAmount);
    }

    public override void ChangeState(StatusEffect newStatusEffect)
    {
        if (newStatusEffect is CorrosionStatusEffect)
        {

        }

        StatusContext.StatusEffect = newStatusEffect;
    }
}
