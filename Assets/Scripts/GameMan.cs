using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public GameObject unitPrefab;
    public GameObject pusherPrefab;
    public GameObject pullerPrefab;
    public GameObject twisterPrefab;
    public GameObject portalPlacerPrefab;

    private ObjectPool unitPool;
    private ObjectPool pusherPool;
    private ObjectPool pullerPool;
    private ObjectPool twisterPool;
    private ObjectPool portalPlacerPool;

	// Use this for initialization
	void Start ()
    {
        unitPool = new ObjectPool(unitPrefab, false, 1);
        pusherPool = new ObjectPool(pusherPrefab, false, 1);
        pullerPool = new ObjectPool(pullerPrefab, false, 1);
        twisterPool = new ObjectPool(twisterPrefab, false, 1);
        portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 1);
    }

    void Update()
    {
        //find out if the user wants to end their turn
    }

    public void PlaceUnit(GameObject location, UnitType type)
    {
        GameObject unitGO = null; 
        switch(type)
        {
            case UnitType.Unit:
                unitGO = unitPool.GetObject();
                break;
            case UnitType.Puller:
                unitGO = pullerPool.GetObject();
                break;
            case UnitType.Pusher:
                unitGO = pusherPool.GetObject();
                break;
            case UnitType.Twister:
                unitGO = twisterPool.GetObject();
                break;
            case UnitType.PortalPlacer:
                unitGO = portalPlacerPool.GetObject();
                break;
        }
        if (unitGO)
        {
            unitGO.GetComponent<Unit>().FindGridElement();
            unitGO.GetComponent<Unit>().SetLocation(location);
            unitGO.GetComponent<Unit>().remainingMoves = 2;
        }
        else
            Debug.Log("GameMan::PlaceUnit() - Insufficient " + type + " units");
    }

    public void ReturnUnit(GameObject unit)
    {
        unit.transform.position = new Vector3(-50, -50, unit.transform.position.z);
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
            case UnitType.PortalPlacer:
                portalPlacerPool.ReturnObject(unit);
                break;
        }
    }

    public void EndLevel()
    {
        // Go to a menu between levels asking if you want to go to the next level, or if you want to return to the main menu
        Debug.Log("next level");
    }

    public void EndTurn()
    {

    }
}
