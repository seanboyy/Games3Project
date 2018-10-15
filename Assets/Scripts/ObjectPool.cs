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
        // Get the current available object in the pool
        GameObject retVal = pool.Pop();
        if (retVal == null)
        {
            if (canGrow)
                retVal = GameObject.Instantiate<GameObject>(prototype);
        }
        retVal.SetActive(true);
        return retVal;
    }

    public void ReturnObject(GameObject returned)
    {
        returned.SetActive(false);
        pool.Push(returned);
    }
}
