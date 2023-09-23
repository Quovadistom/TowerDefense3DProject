using Zenject;

public class TownSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TownTileService>().AsSingle().NonLazy();

        Container.BindFactory<TownTileVisual, TownTileVisual, TownTileVisual.Factory>().FromFactory<PrefabFactory<TownTileVisual>>();
    }
}
