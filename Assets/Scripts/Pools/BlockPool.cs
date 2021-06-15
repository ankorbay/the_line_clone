﻿using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    public static BlockPool Instance { get; private set; }

    [SerializeField]
    private PooledBlock prefab;

    private Queue<PooledBlock> blocksAvailable = new Queue<PooledBlock>();

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
            PooledBlock block = Instantiate(prefab) as PooledBlock;
            block.gameObject.SetActive(false);
            blocksAvailable.Enqueue(block);
        }

    }

    public void Return(PooledBlock block)
    {
        blocksAvailable.Enqueue(block);
    }

    private void Awake()
    {
        Instance = this;
    }
}
