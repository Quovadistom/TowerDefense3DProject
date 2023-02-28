public class FireStatusEffect : StatusEffect
{
    private float m_damageAmount;

    public FireStatusEffect(float damageAmount, float damageRate, BasicEnemy basicEnemy) : base(basicEnemy, damageRate)
    {
        m_damageAmount = damageAmount;
    }

    public override void ApplyEffect()
    {
        Enemy.TakeDamage(m_damageAmount);
    }

    public override void ChangeState(StatusEffect newStatusEffect)
    {
        if (newStatusEffect is CorrosionStatusEffect)
        {

        }
    }
}
