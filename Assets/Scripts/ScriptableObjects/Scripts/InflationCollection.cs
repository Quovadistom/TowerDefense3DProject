using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InflationType
{
    Fire,
    Corrosion,
    Water,
    Electricity,
    Laser
}

public class InflationData
{
    public InflationData(InflationType inflationType, Guid guid, Func<ModuleParent, bool> inflationCheck)
    {
        Type = inflationType;
        Guid = guid;
        InflationCheck = inflationCheck;
    }

    public InflationType Type { get; private set; }
    public Guid Guid { get; private set; }
    public float InflationPercentage { get; set; }
    public Func<ModuleParent, bool> InflationCheck { get; private set; }
}

public class InflationCollection : ScriptableObject
{
    private List<InflationData> m_inflationDataList = new();

    public void InitializeInflationData()
    {
        m_inflationDataList.AddRange(new List<InflationData>()
        {
            new InflationData(
                InflationType.Fire,
                Guid.Parse("fb59f069-7c67-47c8-b717-1cc940eb911a"),
                (moduleParent) => TurretStatusEffectCheck(moduleParent, EffectType.FIRE)),
            new InflationData(
                InflationType.Corrosion,
                Guid.Parse("323224d4-165b-4bc8-ac47-d8da9c08ac66"),
                (moduleParent) => TurretStatusEffectCheck(moduleParent, EffectType.CORROSION)),
            new InflationData(
                InflationType.Water,
                Guid.Parse("ad484b4f-c030-4e76-a554-a1b7eb55b7de"),
                (moduleParent) => TurretStatusEffectCheck(moduleParent, EffectType.WATER)),
            new InflationData(
                InflationType.Electricity,
                Guid.Parse("9fba3109-df4e-4610-9076-0ebc547e56fd"),
                (moduleParent) => TurretStatusEffectCheck(moduleParent, EffectType.ELECTRICITY)),
            new InflationData(
                InflationType.Laser,
                Guid.Parse("4afd6096-ecc2-4efe-b52f-f6c0189a9780"),
                (moduleParent) => moduleParent.HasComponent<TurretLaserModule>())
        });
    }

    private bool TurretStatusEffectCheck(ModuleParent moduleParent, EffectType effectType)
    {
        bool isSuitable = false;
        moduleParent.TryFindAndActOnComponent<TurretStatusEffectModule>((module) =>
        {
            isSuitable = module.CurrentStatusEffect.EffectTypeType == effectType;
        });

        return isSuitable;
    }

    public InflationData GetInflationData(Guid guid) => m_inflationDataList.First(data => data.Guid == guid);
}
