using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Header("Scriptable Objects")]
    public SceneCollection SceneCollection;
    public TurretCollection TurretCollection;
    public TownSettings TownSettings;
    public ResourceCollection ResourceCollection;
    public BlueprintCollection BlueprintCollection;
    public DebugSettings DebugSettings;

    [Header("Factory Assets")]
    public ItemMenuButton ItemMenuButtonPrefab;

    public override void InstallBindings()
    {
        // Scriptables
        Container.BindInterfacesAndSelfTo<SceneCollection>().FromInstance(SceneCollection);
        Container.BindInterfacesAndSelfTo<TurretCollection>().FromInstance(TurretCollection);
        Container.BindInterfacesAndSelfTo<ResourceCollection>().FromInstance(ResourceCollection);
        Container.BindInterfacesAndSelfTo<BlueprintCollection>().FromInstance(BlueprintCollection);
        Container.BindInterfacesAndSelfTo<DebugSettings>().FromInstance(DebugSettings);
        Container.BindInterfacesAndSelfTo<TownSettings>().FromInstance(TownSettings);

        // Services
        Container.BindInterfacesAndSelfTo<SerializationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DifficultyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ModuleModificationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ResourceService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BlueprintService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TowerAvailabilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TownTileService>().AsSingle().NonLazy();

        // Factories
        Container.BindFactory<ItemMenuButton, ItemMenuButton.Factory>().FromComponentInNewPrefab(ItemMenuButtonPrefab);
    }
}
