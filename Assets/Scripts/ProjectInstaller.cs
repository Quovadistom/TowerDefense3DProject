using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public SceneCollection SceneCollection;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneCollection>().FromInstance(SceneCollection);

        Container.BindInterfacesAndSelfTo<DifficultyService>().AsSingle().NonLazy();
    }
}
