using System;

public class TowerStatusEffectModification : Modification<TurretStatusEffectModule>
{
    public StatusEffect StatusEffect;

    protected override Action<TurretStatusEffectModule> ComponentAction => (component) =>
    {
        component.CurrentStatusEffect = StatusEffect;
    };
}