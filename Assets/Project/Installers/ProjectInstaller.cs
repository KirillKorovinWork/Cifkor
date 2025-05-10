using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();
        Container.Bind<IWeatherView>()
                 .FromComponentInHierarchy()
                 .AsSingle();
        Container.BindInterfacesAndSelfTo<WeatherPresenter>().AsSingle();

        Container.Bind<BreedsView>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<BreedsPresenter>().AsSingle();
    }
}
