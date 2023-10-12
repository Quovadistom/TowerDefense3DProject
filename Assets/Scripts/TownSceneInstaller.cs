using System;
using UnityEngine;
using Zenject;

public class TownSceneInstaller : MonoInstaller
{
    [Header("Factory Assets")]
    public TileUpgradeMenuItem TileUpgradeMenuItem;
    public AvailableTileUpgradeButton AvailableTileUpgradeButton;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TownTileService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TownHousingService>().AsSingle().NonLazy();

        Container.BindFactory<TowerTileVisual, Guid, TowerTileVisual, TowerTileVisual.Factory>().FromFactory<PrefabFactory<Guid, TowerTileVisual>>();
        Container.BindFactory<TileUpgradeMenuItem, TileUpgradeMenuItem.Factory>().FromComponentInNewPrefab(TileUpgradeMenuItem);
        Container.BindFactory<Guid, AvailableTileUpgradeButton, AvailableTileUpgradeButton.Factory>().FromComponentInNewPrefab(AvailableTileUpgradeButton);
    }
}
