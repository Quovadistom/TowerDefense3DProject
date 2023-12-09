using System;

public class GeneralInflation : InflationData
{
    public GeneralInflation(InflationType inflationType, Guid guid) : base(inflationType, guid)
    {
    }

    public override bool IsModelParentSuitable(ModuleParent module) => true;

    public override bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent) => true;
}
