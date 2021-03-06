using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SizeReducerPool : MonoBehaviour
{
    public static SizeReducerPool Instance { get; set; }

    PooledSizeReducer.Factory _pooledSizeReducerFactory;

    [Inject]
    public void Construct(PooledSizeReducer.Factory pooledSizeReducerFactory)
    {
        this._pooledSizeReducerFactory = pooledSizeReducerFactory;
    }

    Queue<PooledSizeReducer> sizeReducersAvailable = new Queue<PooledSizeReducer>();


    void Awake()
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
            PooledSizeReducer sizeReducer = _pooledSizeReducerFactory.Create();
            sizeReducer.gameObject.transform.parent = gameObject.transform;
            sizeReducer.gameObject.SetActive(false);
            sizeReducersAvailable.Enqueue(sizeReducer);
        }

    }

    public void Return(PooledSizeReducer sizeReducer)
    {
        sizeReducersAvailable.Enqueue(sizeReducer);
    }
}
