
using UnityEngine;

public class NoneStatusEffect : StatusEffect
{
    public NoneStatusEffect(StatusContext statusContext, float damageRate = 0) : base(statusContext, damageRate)
    {
    }

    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeState(StatusEffect newStatusEffect) 
    {
        Debug.LogError("None");

        StatusContext.StatusEffect = newStatusEffect;
    }
}
