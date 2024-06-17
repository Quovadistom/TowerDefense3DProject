using System;

public class StatusEffectInflation : InflationData
{
    private EffectType m_effectType;

    public StatusEffectInflation(Guid guid, EffectType effectType) : base(guid)
    {
        m_effectType = effectType;
    }

    public override bool IsModelParentSuitable(ModuleParent module)
    {
        bool isSuitable = false;

        module.TryFindAndActOnModule<DamageModule>((suitableModel) =>
        {
            isSuitable = suitableModel.StatusEffect.Value.EffectTypeType == m_effectType;
        });

        return isSuitable;
    }

    public override bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent)
    {
        if (moduleModificationBase is TowerStatusEffectModification modification)
        {
            return modification.StatusEffect.EffectTypeType == m_effectType;
        }

        if (IsModelParentSuitable(moduleParent))
        {
            return moduleModificationBase is TowerRangeModification ||
                moduleModificationBase is TowerDamageModification;
        }

        return false;
    }
}
