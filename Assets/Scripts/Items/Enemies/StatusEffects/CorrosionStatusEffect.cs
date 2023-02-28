public class CorrosionStatusEffect : StatusEffect
{
    public CorrosionStatusEffect(StatusContext statusContext, float damageRate) : base(statusContext, damageRate)
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

        StatusContext.StatusEffect = newStatusEffect;
    }
}
