using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public GameObject unitPrefab;
    public ObjectPool unitPool;

	// Use this for initialization
	void Start ()
    {
        unitPool = new ObjectPool(unitPrefab, false, 3);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlaceUnit(GameObject location)
    {

    }
}
