using System;

public class TowerStatusEffectModification : Modification<DamageModule>
{
    public StatusEffect StatusEffect;

    protected override Action<DamageModule> ComponentAction => (component) =>
    {
        component.StatusEffect.Value = StatusEffect;
    };
}