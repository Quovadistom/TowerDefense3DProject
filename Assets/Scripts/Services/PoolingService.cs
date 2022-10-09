using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingService
{
    private Dictionary<string, ObjectPool<Poolable>> m_availableObjectPools = new Dictionary<string, ObjectPool<Poolable>>();
    private Poolable.Factory m_poolableFactory;

    public PoolingService(Poolable.Factory poolableFactory)
    {
        m_poolableFactory = poolableFactory;
    }

    public ObjectPool<Poolable> AddPool(Poolable poolable,
        Func<Poolable> createFunc,
        Action<Poolable> actionOnGet = null,
        Action<Poolable> actionOnRelease = null,
        Action<Poolable> actionOnDestroy = null,
        bool collectionCheck = true,
        int defaultCapacity = 10, int
        maxSize = 1000)
    {
        ObjectPool<Poolable> objectPool = new ObjectPool<Poolable>(createFunc,
            actionOnGet,
            actionOnRelease,
            actionOnDestroy,
            collectionCheck,
            defaultCapacity,
            maxSize);

        m_availableObjectPools.Add(poolable.GetType().ToString(), objectPool);

        return objectPool;
    }

    public Poolable GetPooledObject(Poolable poolable)
    {
        if (!m_availableObjectPools.TryGetValue(poolable.GetType().ToString(), out ObjectPool<Poolable> objectPool))
        {
            objectPool = AddPool(poolable, 
                () => m_poolableFactory.Create(poolable), 
                (selected) => selected.InitializeObject(), 
                (selected) => selected.ResetObject());
        }

        Poolable pooledObject = objectPool.Get();

        return pooledObject;
    }

    public void ReturnPooledObject(Poolable pooledObject)
    {
        if (m_availableObjectPools.TryGetValue(pooledObject.GetType().ToString(), out ObjectPool<Poolable> objectPool))
        {
            objectPool.Release(pooledObject);
        }
    }
}
