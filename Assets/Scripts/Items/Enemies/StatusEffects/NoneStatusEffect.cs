using UnityEngine;

public class NoneStatusEffect : StatusEffect
{
    public override EffectType EffectTypeType => EffectType.NONE;

    public NoneStatusEffect(float damageRate = 0, float effectTime = Mathf.Infinity) : base(damageRate, effectTime)
    {
    }

    public override void RequestEffectChange(StatusEffect newStatusEffect)
    {
    }

    public override void ApplyEffect(BasicEnemy basicEnemy)
    {
    }
}
