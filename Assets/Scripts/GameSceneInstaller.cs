using System;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Scriptable Objects")]
    public LayerSettings LayerSettings;
    public ColorSettings ColorSettings;
    public PrefabCollection PrefabCollection;

    [Header("Factory Assets")]
    public GameObject SpawnTurretButtonPrefab;
    public TowerModificationButton ModificationButtonPrefab;

    public override void InstallBindings()
    {
        // Scriptables
        Container.BindInterfacesAndSelfTo<LayerSettings>().FromInstance(LayerSettings);
        Container.BindInterfacesAndSelfTo<ColorSettings>().FromInstance(ColorSettings);
        Container.BindInterfacesAndSelfTo<PrefabCollection>().FromInstance(PrefabCollection);
        Container.BindInterfacesAndSelfTo<InflationCollection>().AsSingle().NonLazy();

        // Services
        Container.BindInterfacesAndSelfTo<PoolingService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EnemyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WaveService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BulletService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TouchInputService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SelectionService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DraggingService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TowerService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<InflationService>().AsSingle().NonLazy();

        // Factories
        Container.BindFactory<TowerModule, Guid, TowerModule, TowerModule.Factory>().FromFactory<PrefabFactory<Guid, TowerModule>>();
        Container.BindFactory<Poolable, Poolable, Poolable.Factory>().FromFactory<PrefabFactory<Poolable>>();
        Container.BindFactory<SpawnTowerButton, SpawnTowerButton.Factory>().FromComponentInNewPrefab(SpawnTurretButtonPrefab);
        Container.BindFactory<TowerModificationData, ModificationTree, TowerModificationButton, TowerModificationButton.Factory>().FromComponentInNewPrefab(ModificationButtonPrefab);
    }
}
