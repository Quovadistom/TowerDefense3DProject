using UnityEngine;

public class NoneStatusEffect : StatusEffect
{
    public override EffectType EffectTypeType => EffectType.None;

    public override string EffectName => "None";

    public NoneStatusEffect(float damageRate = 0, float effectTime = Mathf.Infinity) : base(damageRate, effectTime)
    {
    }

    public override bool RequestEffectChange(StatusEffect newStatusEffect)
    {
        return true;
    }

    public override void ApplyEffect(BasicEnemy basicEnemy)
    {
    }
}
