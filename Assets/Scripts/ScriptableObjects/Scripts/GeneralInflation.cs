using System;

public class GeneralInflation : InflationData
{
    public GeneralInflation(Guid guid) : base(guid)
    {
    }

    public override bool IsModelParentSuitable(ModuleParent module) => true;

    public override bool IsModificationSuitable(ModuleModificationBase moduleModificationBase, ModuleParent moduleParent) => true;
}
