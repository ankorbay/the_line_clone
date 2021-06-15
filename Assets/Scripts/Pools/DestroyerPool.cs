using System.Collections.Generic;
using UnityEngine;

public class DestroyerPool : MonoBehaviour
{
    public static DestroyerPool Instance { get; private set; }

    [SerializeField]
    private PooledDestroyer prefab;

    private Queue<PooledDestroyer> destroyersAvailable = new Queue<PooledDestroyer>();

    private void Awake()
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
            PooledDestroyer destroyer = Instantiate(prefab) as PooledDestroyer;
            destroyer.gameObject.SetActive(false);
            destroyersAvailable.Enqueue(destroyer);
        }

    }

    public void Return(PooledDestroyer destroyer)
    {
        destroyersAvailable.Enqueue(destroyer);
    }
}

