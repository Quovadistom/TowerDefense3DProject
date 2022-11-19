using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public LayerSettings LayerSettings;
    public ColorSettings ColorSettings;
    public WaveSettings WaveSettings;
    public PrefabCollection PrefabCollection;

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
        Container.BindInterfacesAndSelfTo<PlacementService>().AsSingle().NonLazy();

        // Factories
        Container.Bind<TurretFactory>().AsSingle();

        Container.BindFactory<TurretInfoComponent, TurretInfoComponent, TurretInfoComponent.Factory>().FromFactory<PrefabFactory<TurretInfoComponent>>();
        Container.BindFactory<Poolable, Poolable, Poolable.Factory>().FromFactory<PrefabFactory<Poolable>>();
    }
}
