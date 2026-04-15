using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base system to reuse objects and save performance.
/// </summary>
public abstract class ObjectPool<T> : Singleton<ObjectPool<T>> where T : MonoBehaviour
{
    #region variables

    [Header("Pool Settings")]
    [SerializeField] protected T prefab;
    private int amount;
    private bool isReady;

    [Header("Pool Cache")]
    private List<T> pooledObjects;

    #endregion

    /// <summary>
    /// Creates a set number of objects and hides them in the pool.
    /// </summary>
    public void PoolObjects(int amount = 0)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("Amount to pool must be non-negative");

        this.amount = amount;
        pooledObjects = new List<T>(amount);
        GameObject newObject;

        for(int i = 0; i < amount; i++)
        {
            newObject = Instantiate(prefab.gameObject, transform); 
            newObject.SetActive(false); 
            pooledObjects.Add(newObject.GetComponent<T>()); 
        }
        isReady = true; 
    }

    /// <summary>
    /// Grabs an inactive object from the pool or creates a new one if none are left.
    /// </summary>
    public T GetPooledObject()
    {
        if (!isReady)
            PoolObjects(1);

        for(int i = 0; i != amount; i++)
            if (!pooledObjects[i].isActiveAndEnabled)
                return pooledObjects[i];

        GameObject newObject = Instantiate(prefab.gameObject, transform);
        newObject.SetActive(false);
        pooledObjects.Add(newObject.GetComponent<T>());
        ++amount;
        return newObject.GetComponent<T>();
    }

    /// <summary>
    /// Disables an object and puts it back into the pool for reuse.
    /// </summary>
    public void ReturnObjectPool(T toBeReturned)
    {
        if (toBeReturned == null)
            return;

        if(!isReady)
        {
            PoolObjects();
            pooledObjects.Add(toBeReturned);
        }

        toBeReturned.gameObject.SetActive(false);
    }
}
