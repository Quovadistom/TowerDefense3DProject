public class NoneStatusEffect : StatusEffect
{
    public override EffectType EffectTypeType => EffectType.None;

    public override string EffectName => "None";

    public override bool RequestEffectChange(StatusEffect newStatusEffect)
    {
        return true;
    }

    public override void ApplyEffect(BasicEnemy basicEnemy)
    {
    }
}
