using System.Collections;
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

    public GameObject unitPiece;
    public GameObject pusherPiece;
    public GameObject pullerPiece;
    public GameObject twisterPiece;
    public GameObject portalPlacerPiece;

    private float prevHorAxis = 0;
    private float prevVerAxis = 0;
    private DoublyLinkedListNode head;

    private bool canInput = true;

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
        if (isLocalPlayer)
        {
            CmdRequestUnit(UnitType.Unit);
            CmdRequestUnit(UnitType.Pusher);
            CmdRequestUnit(UnitType.Puller);
            CmdRequestUnit(UnitType.Twister);
            CmdRequestUnit(UnitType.PortalPlacer);
        }
        if (gameManager == null && isLocalPlayer)
            CmdFindGameManager();
    }

    [Command]
    public void CmdRequestUnit(UnitType type)
    {
        GameObject _unit = null;
        switch (type)
        {
            case UnitType.Unit:
                _unit = Instantiate(unitPrefab, transform);
                NetworkServer.Spawn(_unit);
                unitPiece = _unit;
                unitPiece.SetActive(false);
                break;
            case UnitType.Pusher:
                _unit = Instantiate(pusherPrefab, transform);
                NetworkServer.Spawn(_unit);
                pusherPiece = _unit;
                pusherPiece.SetActive(false);
                break;
            case UnitType.Puller:
                _unit = Instantiate(pullerPrefab, transform);
                NetworkServer.Spawn(_unit);
                pullerPiece = _unit;
                pullerPiece.SetActive(false);
                break;
            case UnitType.Twister:
                _unit = Instantiate(twisterPrefab, transform);
                NetworkServer.Spawn(_unit);
                twisterPiece = _unit;
                twisterPiece.SetActive(false);
                break;
            case UnitType.PortalPlacer:
                _unit = Instantiate(portalPlacerPrefab, transform);
                NetworkServer.Spawn(_unit);
                portalPlacerPiece = _unit;
                portalPlacerPiece.SetActive(false);
                break;
        }
        RpcRequestUnit(type, _unit);
    }

    [ClientRpc]
    public void RpcRequestUnit(UnitType type, GameObject _unit)
    {
        switch (type)
        {
            case UnitType.Unit:
                unitPiece = _unit;
                unitPiece.SetActive(false);
                break;
            case UnitType.Pusher:
                pusherPiece = _unit;
                pusherPiece.SetActive(false);
                switch (identity)
                {
                    case PlayerEnum.Player1:
                        pusherPiece.GetComponent<SpriteRenderer>().sprite = gameManager.pusherSprites[0];
                        break;
                    case PlayerEnum.Player2:
                        pusherPiece.GetComponent<SpriteRenderer>().sprite = gameManager.pusherSprites[1];
                        break;
                }
                break;
            case UnitType.Puller:
                pullerPiece = _unit;
                pullerPiece.SetActive(false);
                switch (identity)
                {
                    case PlayerEnum.Player1:
                        pullerPiece.GetComponent<SpriteRenderer>().sprite = gameManager.pullerSprites[0];
                        break;
                    case PlayerEnum.Player2:
                        pullerPiece.GetComponent<SpriteRenderer>().sprite = gameManager.pullerSprites[1];
                        break;
                }
                break;
            case UnitType.Twister:
                twisterPiece = _unit;
                twisterPiece.SetActive(false);
                switch (identity)
                {
                    case PlayerEnum.Player1:
                        twisterPiece.GetComponent<SpriteRenderer>().sprite = gameManager.twisterSprites[0];
                        break;
                    case PlayerEnum.Player2:
                        twisterPiece.GetComponent<SpriteRenderer>().sprite = gameManager.twisterSprites[1];
                        break;
                }
                break;
            case UnitType.PortalPlacer:
                portalPlacerPiece = _unit;
                portalPlacerPiece.SetActive(false);
                switch (identity)
                {
                    case PlayerEnum.Player1:
                        portalPlacerPiece.GetComponent<SpriteRenderer>().sprite = gameManager.portalPlacerSprites[0];
                        break;
                    case PlayerEnum.Player2:
                        portalPlacerPiece.GetComponent<SpriteRenderer>().sprite = gameManager.portalPlacerSprites[1];
                        break;
                }
                break;
        }

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
        if (FindObjectOfType<MultiMan>())
        {
            gameManager = FindObjectOfType<MultiMan>();
            gameManager.RegisterPlayers();
            activeMenu.activeUIMenu = true;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        if (gameManager == null && isLocalPlayer)
        {
            CmdFindGameManager();
            return;
        }
        if (canInput)
        {
            if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            { activeMenu.HandleHorizontalMovement(Input.GetAxisRaw("Horizontal")); canInput = false; }
            if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            { activeMenu.HandleVerticalMovement(Input.GetAxisRaw("Vertical")); canInput = false; }
            prevHorAxis = Input.GetAxisRaw("Horizontal");
            prevVerAxis = Input.GetAxisRaw("Vertical");

            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            { activeMenu.HandleCrossButton(); canInput = false; }
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            { activeMenu.HandleCircleButton(); canInput = false; }
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            { CmdHandleTriangleButton(); canInput = false; }
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            { activeMenu.HandleSquareButton(); canInput = false; }
        }
        else
            canInput = true;
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
        switch (type)
        {
            case UnitType.Unit:
                DoPlaceUnit(unitPiece, location);
                break;
            case UnitType.Puller:
                DoPlaceUnit(pullerPiece, location);
                break;
            case UnitType.Pusher:
                DoPlaceUnit(pusherPiece, location);
                break;
            case UnitType.Twister:
                DoPlaceUnit(twisterPiece, location);
                break;
            case UnitType.PortalPlacer:
                DoPlaceUnit(portalPlacerPiece, location);
                break;
        }
        RpcPlaceUnit(location, type);
    }

    [ClientRpc]
    public void RpcPlaceUnit(GameObject location, UnitType type)
    {
        switch (type)
        {
            case UnitType.Unit:
                DoPlaceUnit(unitPiece, location);
                break;
            case UnitType.Puller:
                DoPlaceUnit(pullerPiece, location);
                break;
            case UnitType.Pusher:
                DoPlaceUnit(pusherPiece, location);
                break;
            case UnitType.Twister:
                DoPlaceUnit(twisterPiece, location);
                break;
            case UnitType.PortalPlacer:
                DoPlaceUnit(portalPlacerPiece, location);
                break;
        }
    }

    private void DoPlaceUnit(GameObject piece, GameObject location)
    {
        if (piece.activeInHierarchy) return;
        piece.SetActive(true);
        piece.GetComponent<NetworkedUnit>().SetLocation(location);
        piece.GetComponent<NetworkedUnit>().owner = gameObject;
        piece.GetComponent<NetworkedUnit>().remainingMoves = 2;
        if (!isServer)
        {
            if (head != null)
            {
                DoublyLinkedListNode current = head;
                while (current.forward != head)
                    current = current.forward;
                DoublyLinkedListNode newNode = new DoublyLinkedListNode(piece.GetComponent<NetworkedUnit>(), current, head);
                current.forward = newNode;
                head.prev = newNode;
            }
            else
            {
                DoublyLinkedListNode newHead = new DoublyLinkedListNode(piece.GetComponent<NetworkedUnit>());
                newHead.forward = newHead;
                newHead.prev = newHead;
                head = newHead;
            }
        }
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
        UnitType unitType = unit.GetComponent<NetworkedUnit>().unitType;
        switch (unitType)
        {
            case UnitType.Unit:
                DoReturnUnit(unitPiece);
                break;
            case UnitType.Puller:
                DoReturnUnit(pullerPiece);
                break;
            case UnitType.Pusher:
                DoReturnUnit(pusherPiece);
                break;
            case UnitType.Twister:
                DoReturnUnit(twisterPiece);
                break;
            case UnitType.PortalPlacer:
                DoReturnUnit(portalPlacerPiece);
                break;
        }
    }

    private void DoReturnUnit(GameObject piece)
    {
        if (!piece.activeInHierarchy) return;
        piece.SetActive(false);
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
        foreach (NetworkedMenu menu in menus)
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
    public void CmdMovePiece(GameObject newLocation, GameObject oldLocation)
    {
        RpcMovePiece(newLocation, oldLocation);
    }

    [ClientRpc]
    public void RpcMovePiece(GameObject newLocation, GameObject oldLocation)
    {
        NetworkedGridElement nge = oldLocation.GetComponent<NetworkedGridElement>();
        if (!nge) return;
        if (nge.piece.GetComponent<NetworkedUnit>())
            nge.piece.GetComponent<NetworkedUnit>().SetLocation(newLocation);
        else if (nge.piece.GetComponent<NetworkedTrap>())
            nge.piece.GetComponent<NetworkedTrap>().SetLocation(newLocation);
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
            if (current._item == unit)
                break;
            current = current.forward;
        } while (current.forward != head);
        return current;
    }
}
