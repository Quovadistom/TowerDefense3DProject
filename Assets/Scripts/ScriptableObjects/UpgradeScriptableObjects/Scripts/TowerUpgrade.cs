using System;

public abstract class Modification<T> : ModuleModificationBase where T : ModuleBase
{
    protected abstract Action<T> ComponentAction { get; }

    public override bool IsObjectSuitable(ModuleParent component)
    {
        return component.HasComponent<T>();
    }

    public override void TryApplyModification(ModuleParent towerInfoComponent)
    {
        towerInfoComponent.TryFindAndActOnComponent<T>(ComponentAction);
    }
}