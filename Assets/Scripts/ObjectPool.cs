using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    GameObject prototype;
    List<GameObject> pool;
    public bool canGrow;
    private int index;

    public ObjectPool(GameObject prefab, bool resizeable, int count)
    {
        prototype = prefab;
        canGrow = resizeable;
        pool = new List<GameObject>(count);
        index = 0;

        for (int i=0; i<count; i++)
        {
            pool[i] = GameObject.Instantiate<GameObject>(prefab);
        }
    }

    public GameObject GetObject()
    {
        // Get the current available object in the pool
        GameObject retVal = pool[index];
        return retVal;
    }
}
