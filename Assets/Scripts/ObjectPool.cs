using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    GameObject prototype;
    Stack<GameObject> pool;
    public bool canGrow;

    public ObjectPool(GameObject prefab, bool resizeable, int count)
    {
        prototype = prefab;
        canGrow = resizeable;
        pool = new Stack<GameObject>(count);

        for (int i=0; i<count; i++)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(prototype);
            temp.SetActive(false);
            pool.Push(temp);
        }
    }

    public GameObject GetObject()
    {
        GameObject retVal = null;
        // Check to make sure the pool isn't empty
        if (pool.Count != 0)
        {
            // Get the current available object in the pool
            retVal = pool.Pop();
            retVal.SetActive(true);
        }
        else
        {
            if (canGrow)
                retVal = GameObject.Instantiate<GameObject>(prototype);
        }
        return retVal;
    }

    public void ReturnObject(GameObject returned)
    {
        returned.SetActive(false);
        pool.Push(returned);
    }
}
