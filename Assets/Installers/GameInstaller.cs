using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField] GameObject sizeReducerPrefab;
    [SerializeField] GameObject destroyerPrefab;
    [SerializeField] GameObject blockPrefab;


    public override void InstallBindings()
    {
        Container.Bind<Spawner>().FromInstance(FindObjectOfType<Spawner>());
        Container.Bind<Player>().FromInstance(FindObjectOfType<Player>());
        Container.Bind<Score>().FromInstance(FindObjectOfType<Score>());
        Container.Bind<InfoPlaneText>().FromInstance(FindObjectOfType<InfoPlaneText>());
        Container.Bind<TouchPlane>().FromInstance(FindObjectOfType<TouchPlane>());
        Container.Bind<PlayerPositionController>().FromInstance(FindObjectOfType<PlayerPositionController>());
        Container.Bind<GameOverPanel>().FromInstance(FindObjectOfType<GameOverPanel>());

        Container.BindFactory<PooledBlock, PooledBlock.Factory>().FromComponentInNewPrefab(blockPrefab);
        Container.BindFactory<PooledSizeReducer, PooledSizeReducer.Factory>().FromComponentInNewPrefab(sizeReducerPrefab);
        Container.BindFactory<PooledDestroyer, PooledDestroyer.Factory>().FromComponentInNewPrefab(destroyerPrefab);
    }
}