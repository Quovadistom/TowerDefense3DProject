using System;

public abstract class Upgrade<T> : UpgradeBase where T : ComponentBase
{
    protected abstract Action<T> ComponentAction { get; }

    public override bool IsObjectSuitable(ComponentParent component)
    {
        return component.HasComponent<T>();
    }

    public override void TryApplyUpgrade(ComponentParent towerInfoComponent)
    {
        towerInfoComponent.TryFindAndActOnComponent<T>(ComponentAction);
    }
}