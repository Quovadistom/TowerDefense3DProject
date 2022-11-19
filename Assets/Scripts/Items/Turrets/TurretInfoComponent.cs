using Zenject;

public class TurretInfoComponent : ValueComponent
{
    public TurretUpgradeTreeBase UpgradeTreeAsset;

    public class Factory : PlaceholderFactory<TurretInfoComponent, TurretInfoComponent>
    {
    }
}
