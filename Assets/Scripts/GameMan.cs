using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public GameObject unitPrefab;
    public GameObject pusherPrefab;
    public GameObject pullerPrefab;
    public GameObject twisterPrefab;

    private ObjectPool unitPool;
    private ObjectPool pusherPool;
    private ObjectPool pullerPool;
    private ObjectPool twisterPool;

	// Use this for initialization
	void Start ()
    {
        unitPool = new ObjectPool(unitPrefab, false, 1);
        pusherPool = new ObjectPool(pusherPrefab, false, 1);
        pullerPool = new ObjectPool(pullerPrefab, false, 1);
        twisterPool = new ObjectPool(twisterPrefab, false, 1);
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

    public void PlacePuller(GameObject location)
    {
        GameObject unitGO = pullerPool.GetObject();
        if (unitGO)
        {
            unitGO.transform.position = new Vector3(location.transform.position.x - 0.5f, location.transform.parent.transform.position.y - 0.5f, unitGO.transform.position.z);
        }
        else
            Debug.Log("GameMan::PlaceUnit() - no more Pullers available");
    }

    public void PlacePusher(GameObject location)
    {
        GameObject unitGO = pusherPool.GetObject();
        if (unitGO)
        {
            unitGO.transform.position = new Vector3(location.transform.position.x - 0.5f, location.transform.parent.transform.position.y - 0.5f, unitGO.transform.position.z);
        }
        else
            Debug.Log("GameMan::PlaceUnit() - no more Pushers available");
    }

    public void PlaceTwister(GameObject location)
    {
        GameObject unitGO = twisterPool.GetObject();
        if (unitGO)
        {
            unitGO.transform.position = new Vector3(location.transform.position.x - 0.5f, location.transform.parent.transform.position.y - 0.5f, unitGO.transform.position.z);
        }
        else
            Debug.Log("GameMan::PlaceUnit() - no more Twisters available");
    }

    public void ReturnUnit(GameObject unit)
    {
        UnitType unitType = unit.GetComponent<Unit>().unitType;
        switch (unitType)
        {
            case UnitType.Unit:
                unitPool.ReturnObject(unit);
                break;
            case UnitType.Puller:
                pullerPool.ReturnObject(unit);
                break;
            case UnitType.Pusher:
                pusherPool.ReturnObject(unit);
                break;
            case UnitType.Twister:
                twisterPool.ReturnObject(unit);
                break;
        }
    }
}
