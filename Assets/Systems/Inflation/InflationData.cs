using System;

public abstract class InflationData
{
    public InflationData(Guid guid)
    {
        Guid = guid;
    }

    public Guid Guid { get; private set; }
    public float InflationPercentage { get; set; } = 0;

    public abstract bool IsModelParentSuitable(ModuleParent module);
    public abstract bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent);
}
