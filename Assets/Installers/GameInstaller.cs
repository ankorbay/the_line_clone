using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public GameObject sizeReducerPrefab;
    public GameObject destroyerPrefab;
    public GameObject blockPrefab;

    public override void InstallBindings()
    {
        Container.Bind<Spawner>().FromInstance(FindObjectOfType<Spawner>());
        Container.Bind<Player>().FromInstance(FindObjectOfType<Player>());
        Container.Bind<Score>().FromInstance(FindObjectOfType<Score>());
        Container.Bind<InfoPlane>().FromInstance(FindObjectOfType<InfoPlane>());
        Container.Bind<PositionTracker>().FromInstance(FindObjectOfType<PositionTracker>());

        Container.BindFactory<PooledBlock, PooledBlock.Factory>().FromComponentInNewPrefab(blockPrefab);
        Container.BindFactory<PooledSizeReducer, PooledSizeReducer.Factory>().FromComponentInNewPrefab(sizeReducerPrefab);
        Container.BindFactory<PooledDestroyer, PooledDestroyer.Factory>().FromComponentInNewPrefab(destroyerPrefab);
        
    }
}