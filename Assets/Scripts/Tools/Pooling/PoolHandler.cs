using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler
{
    Transform parent;
    GameObject poolObj;

    Action<GameObject, int> initBehaviour;
    Action<GameObject, int> updateBehaviour;
    Action<GameObject, int> despawnBehaviour;

    List<GameObject> objects = new List<GameObject>();

    public PoolHandler(Transform parent, GameObject poolObj)
    {
        this.parent = parent;
        this.poolObj = poolObj;

        initBehaviour = DefaultInitBehaviour;
        updateBehaviour = DefaultUpdateBehaviour;
        despawnBehaviour = DefaultDespawnBehaviour;
    }

    public void SetBehaviours(Action<GameObject, int> init, Action<GameObject, int> update, Action<GameObject, int> despawn)
    {
        initBehaviour = init ?? DefaultInitBehaviour;
        updateBehaviour = update ?? DefaultUpdateBehaviour;
        despawnBehaviour = despawn ?? DefaultDespawnBehaviour;
    }

    public void DefaultInitBehaviour(GameObject obj, int index)
    {

    }

    public void DefaultUpdateBehaviour(GameObject obj, int index)
    {

    }

    public void DefaultDespawnBehaviour(GameObject obj, int index)
    {
        PoolManager.Instance.ReturnToPool(obj);
        objects.RemoveAt(index);
    }

    public void SetPoolToAmount(int newAmount)
    {
        for (int i = 0; i < newAmount; i++)
        {
            if (i >= objects.Count)
            {
                GameObject obj = PoolManager.Instance.GetFromPool(poolObj, parent);
                initBehaviour.Invoke(obj, i);
                objects.Add(obj);
            }

            updateBehaviour.Invoke(objects[i], i);
        }

        for (int i = objects.Count - 1; i >= newAmount; i--)
        {
            despawnBehaviour.Invoke(objects[i], i);
        }
    }
}

