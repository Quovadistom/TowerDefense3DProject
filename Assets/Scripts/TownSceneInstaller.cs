using UnityEngine;
using Zenject;

public class TownSceneInstaller : MonoInstaller
{
    [Header("Factory Assets")]
    public TileUpgradeMenuItem TileUpgradeMenuItem;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TownTileService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TownHousingService>().AsSingle().NonLazy();

        Container.BindFactory<TownTileVisual, TownTileVisual, TownTileVisual.Factory>().FromFactory<PrefabFactory<TownTileVisual>>();
        Container.BindFactory<TileUpgradeMenuItem, TileUpgradeMenuItem.Factory>().FromComponentInNewPrefab(TileUpgradeMenuItem);
    }
}
