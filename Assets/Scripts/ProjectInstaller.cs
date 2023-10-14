using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Header("Scriptable Objects")]
    public SceneCollection SceneCollection;
    public TurretCollection TurretCollection;
    public EnhancementCollection UpgradesCollection;
    public DebugSettings DebugSettings;

    [Header("Factory Assets")]
    public ItemMenuButton ItemMenuButtonPrefab;

    public override void InstallBindings()
    {
        // Scriptables
        Container.BindInterfacesAndSelfTo<SceneCollection>().FromInstance(SceneCollection);
        Container.BindInterfacesAndSelfTo<TurretCollection>().FromInstance(TurretCollection);
        Container.BindInterfacesAndSelfTo<EnhancementCollection>().FromInstance(UpgradesCollection);
        Container.BindInterfacesAndSelfTo<DebugSettings>().FromInstance(DebugSettings);

        // Services
        Container.BindInterfacesAndSelfTo<SerializationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DifficultyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ModuleModificationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EnhancementAvailabilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TowerAvailabilityService>().AsSingle().NonLazy();

        // Factories
        Container.BindFactory<ItemMenuButton, ItemMenuButton.Factory>().FromComponentInNewPrefab(ItemMenuButtonPrefab);
    }
}
