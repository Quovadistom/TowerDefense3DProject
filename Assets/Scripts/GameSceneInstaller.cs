using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Scriptable Objects")]
    public LayerSettings LayerSettings;
    public ColorSettings ColorSettings;
    public WaveSettings WaveSettings;
    public PrefabCollection PrefabCollection;

    [Header("Factory Assets")]
    public GameObject SpawnTurretButtonPrefab;

    public override void InstallBindings()
    {
        // Scriptables
        Container.BindInterfacesAndSelfTo<LayerSettings>().FromInstance(LayerSettings);
        Container.BindInterfacesAndSelfTo<ColorSettings>().FromInstance(ColorSettings);
        Container.BindInterfacesAndSelfTo<WaveSettings>().FromInstance(WaveSettings);
        Container.BindInterfacesAndSelfTo<PrefabCollection>().FromInstance(PrefabCollection);

        // Services
        Container.BindInterfacesAndSelfTo<PoolingService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<EnemyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WaveService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BulletService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TouchInputService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SelectionService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DraggingService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TowerService>().AsSingle().NonLazy();

        // Factories
        Container.BindFactory<TowerInfoComponent, TowerInfoComponent, TowerInfoComponent.Factory>().FromFactory<PrefabFactory<TowerInfoComponent>>();
        Container.BindFactory<Poolable, Poolable, Poolable.Factory>().FromFactory<PrefabFactory<Poolable>>();
        Container.BindFactory<SpawnTowerButton, SpawnTowerButton.Factory>().FromComponentInNewPrefab(SpawnTurretButtonPrefab);
    }
}
