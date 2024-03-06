using System;

public abstract class Modification<T> : ModuleModificationBase where T : ModuleBase
{
    protected abstract Action<T> ComponentAction { get; }

    public override bool IsObjectSuitable(ModuleParent component)
    {
        return component.HasModule<T>();
    }

    public override void TryApplyModification(ModuleParent towerInfoComponent)
    {
        towerInfoComponent.TryFindAndActOnModule<T>(ComponentAction);
    }
}