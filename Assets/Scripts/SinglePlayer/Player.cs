﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public PlayerEnum identity = PlayerEnum.Player1;
    public IGameMan gameManager;

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

    private float prevHorAxis = 0;
    private float prevVerAxis = 0;

    private bool multiplayer = false;

    // These are SyncVars so the menu stays the same across client/server
    public Menu activeMenu;
    public Menu prevMenu;
    [SyncVar]
    public bool activePlayer = false;


    // Use this for initialization
    void Start()
    {
        FindGameManager();
        unitPool = new ObjectPool(unitPrefab, false, 1, transform);
        pusherPool = new ObjectPool(pusherPrefab, false, 1, transform);
        pullerPool = new ObjectPool(pullerPrefab, false, 1, transform);
        twisterPool = new ObjectPool(twisterPrefab, false, 1, transform);
        portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 1, transform);
        if (gameManager == null)
            FindGameManager();
    }

    void FindGameManager()
    {
        if (FindObjectOfType<MultiMan>())
        {
            gameManager = FindObjectOfType<MultiMan>();
            multiplayer = true;
        }
        if (FindObjectOfType<SingleMan>())
        {
            gameManager = FindObjectOfType<SingleMan>();
            multiplayer = false;
        }
        /*
        if (gameManager is MultiMan)
        {
            ((MultiMan)gameManager).CmdRegisterPlayer(gameObject);
        }
        */
    }

    void Update()
    {
        //Debug.Log(connectionToClient + ", " + connectionToServer);
        if (multiplayer && !isLocalPlayer) return;
        if (gameManager == null)
        {
            FindGameManager();
            return;
        }
        if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            activeMenu.HandleHorizontalMovement(Input.GetAxisRaw("Horizontal"));
        if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            activeMenu.HandleVerticalMovement(Input.GetAxisRaw("Vertical"));
        prevHorAxis = Input.GetAxisRaw("Horizontal");
        prevVerAxis = Input.GetAxisRaw("Vertical");

        if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            activeMenu.HandleCrossButton();
        if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            activeMenu.HandleCircleButton();
        if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            activeMenu.HandleTriangleButton();
        if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            activeMenu.HandleSquareButton();
    }

    [Command]
    public void CmdHandleCrossButton()
    {
        activeMenu.HandleCrossButton();
    }

    [Command]
    public void CmdHandleCircleButton()
    {
        activeMenu.HandleCircleButton();
    }

    [Command]
    public void CmdHandleTriangleButton()
    {
        gameManager.EndTurn();
    }

    [Command]
    public void CmdHandleSquareButton()
    { 
        activeMenu.HandleSquareButton();
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
            uScript.owner = gameObject;
            uScript.SetLocation(location);
            uScript.remainingMoves = 2;
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

    // This get's called by ContextMenu
    public void SetActiveMenu(Menu newMenu)
    {
    }

    public void SetActiveMenu(NetworkedMenu newMenu)
    {
        if (isLocalPlayer)
        {
            CmdSetActiveMenu(newMenu.gameObject);
        }
    }

    // This gets called only on the server version of this object
    [Command]
    public void CmdSetActiveMenu(GameObject newMenu)
    {
        prevMenu = activeMenu;
        activeMenu = newMenu.GetComponent<Menu>();
        RpcSetActiveMenu(newMenu);
    }

    // This gets called on every client
    [ClientRpc]
    public void RpcSetActiveMenu(GameObject newMenu)
    {
        prevMenu = activeMenu;
        activeMenu = newMenu.GetComponent<Menu>();
    }
}