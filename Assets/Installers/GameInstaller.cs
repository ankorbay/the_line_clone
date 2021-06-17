using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Spawner>().FromInstance(FindObjectOfType<Spawner>());
        Container.Bind<Player>().FromInstance(FindObjectOfType<Player>());
        Container.Bind<Score>().FromInstance(FindObjectOfType<Score>());
        Container.Bind<InfoPlane>().FromInstance(FindObjectOfType<InfoPlane>());
    }
}