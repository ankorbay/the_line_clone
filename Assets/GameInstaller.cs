using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameController>();
        Container.Bind<Spawner>().AsTransient();
        Container.Bind<Player>().AsTransient();
        Container.Bind<Score>().AsTransient();
        Container.Bind<InfoPlane>().AsTransient();
    }
}