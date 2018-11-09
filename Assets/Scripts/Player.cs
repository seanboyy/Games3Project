﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnum
{
    Player1,
    Player2, 
    none
}

public class Player : MonoBehaviour
{
    public PlayerEnum identity = PlayerEnum.Player1;

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

    public void PlaceUnit(GameObject location, UnitType type)
    {
        GameObject unitGO = null;
        switch (type)
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
            Unit uScript = unitGO.GetComponent<Unit>();
            uScript.SetLocation(location);
            uScript.remainingMoves = 2;
            uScript.owner = gameObject;
        }
        else
            Debug.Log("Player::PlaceUnit() - Insufficient " + type + " units");
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
}
