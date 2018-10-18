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
        GameObject unitGO = unitPool.GetObject();
        if (unitGO)
        {
            unitGO.transform.position = new Vector3(location.transform.position.x - 0.5f, location.transform.parent.transform.position.y - 0.5f, unitGO.transform.position.z);
            if (unitGO.GetComponent<Unit>())
            {
                Debug.Log("GameMan::PlaceUnit() - Assigned a Grid Element to newly spawned unit: " + unitGO.GetComponent<Unit>().FindGridElement());
            }
        }
        else
            Debug.Log("GameMan::PlaceUnit() - no more GenericUnits available");
    }
}
