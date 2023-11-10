using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Header("Scriptable Objects")]
    public SceneCollection SceneCollection;
    public TurretCollection TurretCollection;
    public ModificationCollection ModificationsCollection;
    public DebugSettings DebugSettings;

    [Header("Factory Assets")]
    public ItemMenuButton ItemMenuButtonPrefab;

    public override void InstallBindings()
    {
        // Scriptables
        Container.BindInterfacesAndSelfTo<SceneCollection>().FromInstance(SceneCollection);
        Container.BindInterfacesAndSelfTo<TurretCollection>().FromInstance(TurretCollection);
        Container.BindInterfacesAndSelfTo<ModificationCollection>().FromInstance(ModificationsCollection);
        Container.BindInterfacesAndSelfTo<DebugSettings>().FromInstance(DebugSettings);

        // Services
        Container.BindInterfacesAndSelfTo<SerializationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DifficultyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ModuleModificationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ModificationAvailabilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TowerAvailabilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();

        // Factories
        Container.BindFactory<ItemMenuButton, ItemMenuButton.Factory>().FromComponentInNewPrefab(ItemMenuButtonPrefab);
    }
}
