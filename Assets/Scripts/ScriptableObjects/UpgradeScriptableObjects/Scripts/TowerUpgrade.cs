using System;

public abstract class Upgrade<T> : ModuleModificationBase where T : ModuleBase
{
    protected abstract Action<T> ComponentAction { get; }

    public override bool IsObjectSuitable(ModuleParent component)
    {
        return component.HasComponent<T>();
    }

    public override void TryApplyUpgrade(ModuleParent towerInfoComponent)
    {
        towerInfoComponent.TryFindAndActOnComponent<T>(ComponentAction);
    }
}