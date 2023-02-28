public class CorrosionStatusEffect : StatusEffect
{
    public CorrosionStatusEffect(float damageRate, BasicEnemy basicEnemy) : base(basicEnemy, damageRate)
    {
    }

    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeState(StatusEffect newStatusEffect)
    {
        if (newStatusEffect is FireStatusEffect)
        {

        }
    }
}
