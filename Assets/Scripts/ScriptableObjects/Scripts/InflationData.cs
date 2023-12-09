using System;

public abstract class InflationData
{
    public InflationData(InflationType inflationType, Guid guid)
    {
        Type = inflationType;
        Guid = guid;
    }

    public InflationType Type { get; private set; }
    public Guid Guid { get; private set; }
    public float InflationPercentage { get; set; } = 0;

    public abstract bool IsModelParentSuitable(ModuleParent module);
    public abstract bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent);
}
