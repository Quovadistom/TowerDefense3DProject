
using UnityEngine;

public class NoneStatusEffect : StatusEffect
{
    public NoneStatusEffect(BasicEnemy basicEnemy, float damageRate = 0) : base(basicEnemy, damageRate)
    {
    }

    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeState(StatusEffect newStatusEffect) { }
}
