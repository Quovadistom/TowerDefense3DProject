using System;

public class StatusEffectInflation : InflationData
{
    private EffectType m_effectType;

    public StatusEffectInflation(InflationType inflationType, Guid guid, EffectType effectType) : base(inflationType, guid)
    {
        m_effectType = effectType;
    }

    public override bool IsModelParentSuitable(ModuleParent module)
    {
        bool isSuitable = false;

        module.TryFindAndActOnModule<TurretStatusEffectModule>((suitableModel) =>
        {
            isSuitable = suitableModel.CurrentStatusEffect.EffectTypeType == m_effectType;
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
