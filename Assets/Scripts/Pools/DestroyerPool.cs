using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DestroyerPool : MonoBehaviour
{
    public static DestroyerPool Instance { get; set; }

    PooledDestroyer.Factory _pooledDestroyerFactory;

    [Inject]
    public void Construct(PooledDestroyer.Factory pooledDestroyerFactory)
    {
        this._pooledDestroyerFactory = pooledDestroyerFactory;
    }

    Queue<PooledDestroyer> destroyersAvailable = new Queue<PooledDestroyer>();


    void Awake()
    {
        Instance = this;
    }


    public PooledDestroyer Get()
    {
        if (destroyersAvailable.Count == 0)
        {
            AddDestroyers(1);
        }

        return destroyersAvailable.Dequeue();
    }

    public void AddDestroyers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PooledDestroyer destroyer = _pooledDestroyerFactory.Create();
            destroyer.gameObject.transform.parent = gameObject.transform;
            destroyer.gameObject.SetActive(false);
            destroyersAvailable.Enqueue(destroyer);
        }

    }

    public void Return(PooledDestroyer destroyer)
    {
        destroyersAvailable.Enqueue(destroyer);
    }
}

