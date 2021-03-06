using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BlockPool : MonoBehaviour
{
    public static BlockPool Instance { get; set; }

    PooledBlock.Factory _pooledBlockFactory;

    [Inject]
    public void Construct(PooledBlock.Factory pooledBlockFactory)
    {
        this._pooledBlockFactory = pooledBlockFactory;
    }

    Queue<PooledBlock> blocksAvailable = new Queue<PooledBlock>();


    void Awake()
    {
        Instance = this;
    }


    public PooledBlock Get()
    {
        if (blocksAvailable.Count == 0)
        {
            AddBlocks(1);
        }

        return blocksAvailable.Dequeue();
    }

    public void AddBlocks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PooledBlock block = _pooledBlockFactory.Create();
            block.gameObject.transform.parent = gameObject.transform;
            block.gameObject.SetActive(false);
            blocksAvailable.Enqueue(block);
        }

    }

    public void Return(PooledBlock block)
    {
        blocksAvailable.Enqueue(block);
    }
}
