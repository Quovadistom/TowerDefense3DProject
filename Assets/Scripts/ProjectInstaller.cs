using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Header("Scriptable Objects")]
    public SceneCollection SceneCollection;
    public TurretCollection TurretCollection;
    public BoostCollection UpgradesCollection;

    [Header("Factory Assets")]
    public ItemMenuButton ItemMenuButtonPrefab;

    public override void InstallBindings()
    {
        // Scriptables
        Container.BindInterfacesAndSelfTo<SceneCollection>().FromInstance(SceneCollection);
        Container.BindInterfacesAndSelfTo<TurretCollection>().FromInstance(TurretCollection);
        Container.BindInterfacesAndSelfTo<BoostCollection>().FromInstance(UpgradesCollection);

        // Services
        Container.BindInterfacesAndSelfTo<SerializationService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DifficultyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TowerBoostService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MenuService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoostAvailabilityService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameBoostService>().AsSingle().NonLazy();

        // Factories
        Container.BindFactory<ItemMenuButton, ItemMenuButton.Factory>().FromComponentInNewPrefab(ItemMenuButtonPrefab);
    }
}
