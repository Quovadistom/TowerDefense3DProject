using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public SceneCollection SceneCollection;
    public TurretCollection TurretCollection;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DifficultyService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SceneCollection>().FromInstance(SceneCollection);
        Container.BindInterfacesAndSelfTo<TurretCollection>().FromInstance(TurretCollection);
    }
}
