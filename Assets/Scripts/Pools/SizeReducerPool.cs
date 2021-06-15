using System.Collections.Generic;
using UnityEngine;

public class SizeReducerPool : MonoBehaviour
{
    public static SizeReducerPool Instance { get; private set; }

    [SerializeField] private PooledSizeReducer prefab;

    private Queue<PooledSizeReducer> sizeReducersAvailable = new Queue<PooledSizeReducer>();

    private void Awake()
    {
        Instance = this;
    }

    public PooledSizeReducer Get()
    {
        if (sizeReducersAvailable.Count == 0)
        {
            AddSizeReducers(1);
        }

        return sizeReducersAvailable.Dequeue();
    }

    public void AddSizeReducers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PooledSizeReducer sizeReducer = Instantiate(prefab) as PooledSizeReducer;
            sizeReducer.gameObject.SetActive(false);
            sizeReducersAvailable.Enqueue(sizeReducer);
        }

    }

    public void Return(PooledSizeReducer sizeReducer)
    {
        sizeReducersAvailable.Enqueue(sizeReducer);
    }
}
