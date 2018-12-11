﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkedPlayer : NetworkBehaviour
{
    [SyncVar]
    public PlayerEnum identity = PlayerEnum.Player1;
    public MultiMan gameManager;

    public GameObject unitPrefab;
    public GameObject pusherPrefab;
    public GameObject pullerPrefab;
    public GameObject twisterPrefab;
    public GameObject portalPlacerPrefab;

    [HideInInspector]
    public GameObject unitPiece;
    [HideInInspector]
    public GameObject pusherPiece;
    [HideInInspector]
    public GameObject pullerPiece;
    [HideInInspector]
    public GameObject twisterPiece;
    [HideInInspector]
    public GameObject portalPlacerPiece;

    private ObjectPool unitPool;
    private ObjectPool pusherPool;
    private ObjectPool pullerPool;
    private ObjectPool twisterPool;
    private ObjectPool portalPlacerPool;

    private float prevHorAxis = 0;
    private float prevVerAxis = 0;
    private DoublyLinkedListNode head;

    // These are SyncVars so the menu stays the same across client/server
    public NetworkedMenu activeMenu;
    public NetworkedMenu prevMenu;
    [SyncVar]
    public bool activePlayer = false;


    // Use this for initialization
    void Start()
    {
        activeMenu = FindObjectOfType<NetworkedGridMenu>();
        if (isLocalPlayer) CmdFindGameManager();
        if (isServer)
        { 
            unitPool = new ObjectPool(unitPrefab, false, 1, transform);
            GameObject unit = unitPool.GetObject();
            NetworkServer.Spawn(unit);
            unitPool.ReturnObject(unit);

            pusherPool = new ObjectPool(pusherPrefab, false, 1, transform);
            GameObject pusher = pusherPool.GetObject();
            NetworkServer.Spawn(pusher);
            pusherPool.ReturnObject(pusher);

            pullerPool = new ObjectPool(pullerPrefab, false, 1, transform);
            GameObject puller = pullerPool.GetObject();
            NetworkServer.Spawn(puller);
            pullerPool.ReturnObject(puller);

            twisterPool = new ObjectPool(twisterPrefab, false, 1, transform);
            GameObject twister = twisterPool.GetObject();
            NetworkServer.Spawn(twister);
            twisterPool.ReturnObject(twister);

            portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 1, transform);
            GameObject portalPlacer = portalPlacerPool.GetObject();
            NetworkServer.Spawn(portalPlacer);
            portalPlacerPool.ReturnObject(portalPlacer);
        }
        if (isLocalPlayer)
        {
            unitPool = new ObjectPool(unitPrefab, false, 0, transform);
            pusherPool = new ObjectPool(pusherPrefab, false, 0, transform);
            pullerPool = new ObjectPool(pullerPrefab, false, 0, transform);
            twisterPool = new ObjectPool(twisterPrefab, false, 0, transform);
            portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 0, transform);

        }
        if (gameManager == null && isLocalPlayer)
            CmdFindGameManager();
    }

    [Command]
    public void CmdFindGameManager()
    {
        RpcFindGameManager();
    }

    [ClientRpc]
    public void RpcFindGameManager()
    {
        activeMenu = FindObjectOfType<NetworkedGridMenu>();
        FindGameManager();
    }

    void FindGameManager()
    {
        gameManager = FindObjectOfType<MultiMan>();
        if (gameManager) gameManager.RegisterPlayers();
        activeMenu.activeUIMenu = true;   
    }

    void Update()
    {
        Debug.Log(gameObject.name + ": " + netId);
        if (!isLocalPlayer) return;
        if (gameManager == null && isLocalPlayer)
        {
            CmdFindGameManager();
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
            CmdHandleTriangleButton();
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
        RpcEndTurn();
    }

    [ClientRpc]
    public void RpcEndTurn()
    {
        gameManager.EndTurn();
    }

    [Command]
    public void CmdHandleSquareButton()
    {
        activeMenu.HandleSquareButton();
    }

    [Command]
    public void CmdPlaceUnit(GameObject location, UnitType type)
    {
        RpcPlaceUnit(location, type);
    }

    [ClientRpc]
    public void RpcPlaceUnit(GameObject location, UnitType type)
    {
        PlaceUnit(location, type);
    }

    public void PlaceUnit(GameObject location, UnitType type)
    {
        GameObject unitGO = null;
        switch (type)
        {
            case UnitType.Unit:
                unitGO = unitPool.GetObject();
                unitPiece = unitGO;
                break;
            case UnitType.Puller:
                unitGO = pullerPool.GetObject();
                pullerPiece = unitGO;
                break;
            case UnitType.Pusher:
                unitGO = pusherPool.GetObject();
                pusherPiece = unitGO;
                break;
            case UnitType.Twister:
                unitGO = twisterPool.GetObject();
                twisterPiece = unitGO;
                break;
            case UnitType.PortalPlacer:
                unitGO = portalPlacerPool.GetObject();
                portalPlacerPiece = unitGO;
                break;
        }
        if (unitGO)
        {
            NetworkedUnit uScript = unitGO.GetComponent<NetworkedUnit>();
            uScript.owner = gameObject;
            uScript.SetLocation(location);
            uScript.remainingMoves = 2;
            // Set up linked list
            if (head != null)
            {
                DoublyLinkedListNode current = head;
                while (current.forward != head)
                    current = current.forward;
                DoublyLinkedListNode newNode = new DoublyLinkedListNode(unitGO.GetComponent<Unit>(), current, head);
                current.forward = newNode;
                head.prev = newNode;
            }
            else
            {
                DoublyLinkedListNode newHead = new DoublyLinkedListNode(unitGO.GetComponent<Unit>());
                newHead.forward = newHead;
                newHead.prev = newHead;
                head = newHead;
            }

        }
        else
            Debug.Log("Player::PlaceUnit() - Insufficient " + type + " units");
    }
    
    [Command]
    public void CmdReturnUnit(GameObject unit)
    {
        RpcReturnUnit(unit);
    }

    [ClientRpc]
    public void RpcReturnUnit(GameObject unit)
    {
        ReturnUnit(unit);
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

    // This gets called by ContextMenu
    public void SetActiveMenu(NetworkedMenu newMenu)
    {
        if (isLocalPlayer)
        {
            CmdSetActiveMenu(newMenu.gameObject);
        }
    }

    public void SetActiveMenu(GameObject newMenu)
    {
        prevMenu = activeMenu;
        NetworkedMenu[] menus = newMenu.GetComponents<NetworkedMenu>();
        foreach(NetworkedMenu menu in menus)
        {
            if (menu.activeUIMenu)
            {
                activeMenu = menu;
                break;
            }
        }
    }

    // This gets called only on the server version of this object
    [Command]
    public void CmdSetActiveMenu(GameObject newMenu)
    {
        prevMenu = activeMenu;
        NetworkedMenu[] menus = newMenu.GetComponents<NetworkedMenu>();
        foreach (NetworkedMenu menu in menus)
        {
            if (menu.activeUIMenu)
            {
                activeMenu = menu;
                break;
            }
        }
        RpcSetActiveMenu(newMenu);
    }

    // This gets called on every client
    [ClientRpc]
    public void RpcSetActiveMenu(GameObject newMenu)
    {
        prevMenu = activeMenu;
        NetworkedMenu[] menus = newMenu.GetComponents<NetworkedMenu>();
        foreach (NetworkedMenu menu in menus)
        {
            if (menu.activeUIMenu)
            {
                activeMenu = menu;
                break;
            }
        }
    }

    [Command]
    public void CmdMovePiece(GameObject location, GameObject piece)
    {
        NetworkedUnit unit = piece.GetComponent<NetworkedUnit>() ?? null;
        NetworkedTrap trap = piece.GetComponent<NetworkedTrap>() ?? null;
        if (unit != null)
        {
            unit.SetLocation(location);
        }
        else
        {
            Debug.Log("No piece here!" + piece);
        }
        if (trap != null)
        {
            trap.SetLocation(location);
        }
        else
        {
            Debug.Log("No trap here!" + piece);
        }
        Debug.Log(piece);
        RpcMovePiece(location, piece);
    }

    [ClientRpc]
    public void RpcMovePiece(GameObject location, GameObject piece)
    {
        if (piece == null) return;
        NetworkedUnit unit = piece.GetComponent<NetworkedUnit>() ?? null;
        NetworkedTrap trap = piece.GetComponent<NetworkedTrap>() ?? null;
        if(unit != null)
        {
            unit.SetLocation(location);
        }
        else
        {
            Debug.Log("No piece here!" + piece);
        }
        if(trap != null)
        {
            trap.SetLocation(location);
        }
        else
        {
            Debug.Log("No trap here!" + piece);
        }
    }

    [Command]
    public void CmdHandleTwist(GameObject twister, GameObject location)
    {
        RpcHandleTwist(twister, location);
    }

    [ClientRpc]
    public void RpcHandleTwist(GameObject twister, GameObject location)
    {
        twister.GetComponent<NetworkedTwister>().TwistBoard(location);
    }

    public void RotateLeft(NetworkedGridElement selectedGE)
    {
        DoublyLinkedListNode curNode = head;
        if (selectedGE.piece && selectedGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedUnit)
        {
            NetworkedUnit selectedUnit = selectedGE.piece.GetComponent<NetworkedUnit>();
            curNode = FindNode(selectedUnit);
        }

        if (activeMenu is NetworkedGridMenu)
            ((NetworkedGridMenu)activeMenu).ChangeElementSelected(curNode.forward.item.GetComponent<Unit>().gridElement.gameObject);
    }

    public void RotateRight(NetworkedGridElement selectedGE)
    {
        DoublyLinkedListNode curNode = head;
        if (selectedGE.piece && selectedGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedUnit)
        {
            NetworkedUnit selectedUnit = selectedGE.piece.GetComponent<NetworkedUnit>();
            curNode = FindNode(selectedUnit);
        }

        if (activeMenu is NetworkedGridMenu)
            ((NetworkedGridMenu)activeMenu).ChangeElementSelected(curNode.prev.item.GetComponent<Unit>().gridElement.gameObject);
    }

    private DoublyLinkedListNode FindNode(NetworkedUnit unit)
    {
        DoublyLinkedListNode current = head;
        do
        {
            if (current.item == unit)
                break;
            current = current.forward;
        } while (current.forward != head);
        return current;
    }
}
