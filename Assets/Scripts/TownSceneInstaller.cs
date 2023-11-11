using System;
using UnityEngine;
using Zenject;

public class TownSceneInstaller : MonoInstaller
{
    [Header("Factory Assets")]
    public TileModificationMenuItem TileModificationMenuItem;
    public AvailableTileModificationButton AvailableTileModificationButton;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TownHousingService>().AsSingle().NonLazy();

        Container.BindFactory<TowerTileVisual, Guid, TowerTileVisual, TowerTileVisual.Factory>().FromFactory<PrefabFactory<Guid, TowerTileVisual>>();
        Container.BindFactory<TileModificationMenuItem, TileModificationMenuItem.Factory>().FromComponentInNewPrefab(TileModificationMenuItem);
        Container.BindFactory<Guid, AvailableTileModificationButton, AvailableTileModificationButton.Factory>().FromComponentInNewPrefab(AvailableTileModificationButton);
    }
}
