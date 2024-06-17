public class FireStatusEffect : StatusEffect
{
    public override EffectType EffectTypeType => EffectType.Fire;

    public override string EffectName => "Fire";

    public override void ApplyEffect(BasicEnemy basicEnemy)
    {
        basicEnemy.TakeDamage(Damage);
    }

    public override bool RequestEffectChange(StatusEffect newStatusEffect)
    {
        //if (newStatusEffect is CorrosionStatusEffect)
        //{

        //}

        return true;
    }
}
