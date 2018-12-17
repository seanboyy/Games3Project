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
    [SerializeField]
    private bool onLocalMulti = false;

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
        if (gameManager == null && isLocalPlayer)
            CmdFindGameManager();
        if (hasAuthority && !onLocalMulti)
        { 
            NetworkedPlayer[] otherPlayers = FindObjectsOfType<NetworkedPlayer>();
            foreach (NetworkedPlayer player in otherPlayers)
            {
                if (player != this && player.hasAuthority)
                {
                    onLocalMulti = true;
                    player.onLocalMulti = true;
                    break;
                }
            }
        }
    }

    // Set up the piece pool server side
    [Command]
    public void CmdRequestUnit(UnitType type)
    {
        GameObject _unit = null;
        switch (type)
        {
            case UnitType.Unit:
                if (!unitPiece)
                {
                    unitPiece = Instantiate(unitPrefab, transform);
                    unitPiece.name = "Unit ";
                    NetworkServer.Spawn(unitPiece);
                }
                _unit = unitPiece;
                unitPiece.SetActive(false);
                break;
            case UnitType.Pusher:
                if (!pusherPiece)
                {
                    pusherPiece = Instantiate(pusherPrefab, transform);
                    pusherPiece.name = "Pusher ";
                    NetworkServer.Spawn(pusherPiece);
                }
                _unit = pusherPiece;
                pusherPiece.SetActive(false);
                break;
            case UnitType.Puller:
                if (!pullerPiece)
                {
                    pullerPiece = Instantiate(pullerPrefab, transform);
                    pullerPiece.name = "Puller ";
                    NetworkServer.Spawn(pullerPiece);
                }
                _unit = pullerPiece;
                pullerPiece.SetActive(false);
                break;
            case UnitType.Twister:
                if (!twisterPiece)
                {
                    twisterPiece = Instantiate(twisterPrefab, transform);
                    twisterPiece.name = "Twister ";
                    NetworkServer.Spawn(twisterPiece);
                }
                _unit = twisterPiece;
                twisterPiece.SetActive(false);
                break;
            case UnitType.Portalist:
                if (!portalPlacerPiece)
                {
                    portalPlacerPiece = Instantiate(portalPlacerPrefab, transform);
                    portalPlacerPiece.name = "Portalist ";
                    NetworkServer.Spawn(portalPlacerPiece);
                }
                _unit = portalPlacerPiece;
                portalPlacerPiece.SetActive(false);
                break;
        }
        RpcRequestUnit(type, _unit);
    }

    // Set up the piece pool client side
    [ClientRpc]
    public void RpcRequestUnit(UnitType type, GameObject _unit)
    {
        switch (type)
        {
            case UnitType.Unit:
                unitPiece = _unit;
                unitPiece.SetActive(false);
                switch (identity)
                {
                    case PlayerEnum.Player1:
                        unitPiece.GetComponent<SpriteRenderer>().sprite = gameManager.unitSprites[0];
                        break;
                    case PlayerEnum.Player2:
                        unitPiece.GetComponent<SpriteRenderer>().sprite = gameManager.unitSprites[1];
                        break;
                };
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
                };
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
                };
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
                };
                break;
            case UnitType.Portalist:
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
                };
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
        gameManager = FindObjectOfType<MultiMan>();
        if (gameManager)
        {
            gameManager.RegisterPlayers();
            if (activeMenu == null)
                activeMenu = FindObjectOfType<NetworkedGridMenu>();
            activeMenu.activeUIMenu = true;
        }
    }

    void Update()
    {
        if (gameManager == null)
        {
            FindGameManager();
            return;
        }
        if (!isLocalPlayer) return;
        if (canInput && !(onLocalMulti && !activePlayer))
        {
            // Try to find the active menu and if you still can't, bail out
            if (activeMenu == null) { activeMenu = FindObjectOfType<NetworkedGridMenu>(); activeMenu.activeUIMenu = true; }
            if (activeMenu == null) { Debug.Log("Active Menu not yet found"); return; }

            if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            { activeMenu.HandleHorizontalMovement(Input.GetAxisRaw("Horizontal")); canInput = false; }
            if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            { activeMenu.HandleVerticalMovement(Input.GetAxisRaw("Vertical")); canInput = false; }
            prevHorAxis = Input.GetAxisRaw("Horizontal");
            prevVerAxis = Input.GetAxisRaw("Vertical");
            if (!activePlayer) return;
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            { activeMenu.HandleCrossButton(); canInput = false; }
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            { activeMenu.HandleCircleButton(); canInput = false; }
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            { CmdHandleTriangleButton(); canInput = false; }
            if (isLocalPlayer && activePlayer && Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            { activeMenu.HandleSquareButton(); canInput = false; }
            if (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.JoystickButton4))
            { activeMenu.HandleLeftShoulderBumper(); canInput = false; }
            if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.JoystickButton5))
            { activeMenu.HandleRightShoulderBumper(); canInput = false; }

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
        if (!gameManager) FindGameManager();
        gameManager.EndTurn();
    }

    [Command]
    public void CmdHandleSquareButton()
    {
        activeMenu.HandleSquareButton();
    }

    // Get a piece server side
    [Command]
    public void CmdPlaceUnit(GameObject location, UnitType type)
    {
        switch (type)
        {
            case UnitType.Unit:
                if (unitPiece)
                    DoPlaceUnit(unitPiece, location);
                else
                {
                    unitPiece = Instantiate(unitPrefab, transform);
                    unitPiece.name = "Unit " + identity;
                    NetworkServer.Spawn(unitPiece);
                    RpcRequestUnit(UnitType.Unit, unitPiece);
                    DoPlaceUnit(unitPiece, location);
                }
                break;
            case UnitType.Puller:
                if (pullerPiece)
                    DoPlaceUnit(pullerPiece, location);
                else
                {
                    pullerPiece = Instantiate(pullerPrefab, transform);
                    pullerPiece.name = "Puller " + identity;
                    NetworkServer.Spawn(pullerPiece);
                    RpcRequestUnit(UnitType.Puller, pullerPiece);
                    DoPlaceUnit(pullerPiece, location);
                }
                break;
            case UnitType.Pusher:
                if (pusherPiece)
                    DoPlaceUnit(pusherPiece, location);
                else
                {
                    pusherPiece = Instantiate(pusherPrefab, transform);
                    pusherPiece.name = "Pusher " + identity;
                    NetworkServer.Spawn(pusherPiece);
                    RpcRequestUnit(UnitType.Pusher, pusherPiece);
                    DoPlaceUnit(pusherPiece, location);
                }
                break;
            case UnitType.Twister:
                if (twisterPiece)
                    DoPlaceUnit(twisterPiece, location);
                else
                {
                    twisterPiece = Instantiate(twisterPrefab, transform);
                    twisterPiece.name = "Twister " + identity;
                    NetworkServer.Spawn(twisterPiece);
                    RpcRequestUnit(UnitType.Twister, twisterPiece);
                    DoPlaceUnit(twisterPiece, location);
                }
                break;
            case UnitType.Portalist:
                if (portalPlacerPiece)
                    DoPlaceUnit(portalPlacerPiece, location);
                else
                {
                    portalPlacerPiece = Instantiate(portalPlacerPrefab, transform);
                    portalPlacerPiece.name = "Portalist " + identity;
                    NetworkServer.Spawn(portalPlacerPiece);
                    RpcRequestUnit(UnitType.Portalist, portalPlacerPiece);
                    DoPlaceUnit(portalPlacerPiece, location);
                }
                break;
        }
        RpcPlaceUnit(location, type);
    }

    // Get a piece client side
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
            case UnitType.Portalist:
                DoPlaceUnit(portalPlacerPiece, location);
                break;
        }
    }

    private void DoPlaceUnit(GameObject piece, GameObject location)
    {
        if (piece.activeInHierarchy) return;
        piece.SetActive(true);
        piece.name = "" + piece.GetComponent<NetworkedUnit>().unitType + identity;
        piece.GetComponent<NetworkedUnit>().SetLocation(location);
        piece.GetComponent<NetworkedUnit>().owner = gameObject;
        piece.GetComponent<NetworkedUnit>().remainingMoves = 2;
        if (isLocalPlayer)
        {
            if (head != null)
            {
                DoublyLinkedListNode current = head;
                while (current.forward != head)
                    current = current.forward;
                NetworkedUnit unit = piece.GetComponent<NetworkedUnit>();
                DoublyLinkedListNode newNode = new DoublyLinkedListNode(unit, current, head);
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
            case UnitType.Portalist:
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
        if (curNode == null) return;
        if (activeMenu is NetworkedGridMenu)
            ((NetworkedGridMenu)activeMenu).ChangeElementSelected(curNode.forward._item.GetComponent<NetworkedUnit>().gridElement.gameObject);
    }

    public void RotateRight(NetworkedGridElement selectedGE)
    {
        DoublyLinkedListNode curNode = head;
        if (selectedGE.piece && selectedGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedUnit)
        {
            NetworkedUnit selectedUnit = selectedGE.piece.GetComponent<NetworkedUnit>();
            curNode = FindNode(selectedUnit);
        }
        if (curNode == null) return;
        if (activeMenu is NetworkedGridMenu)
            ((NetworkedGridMenu)activeMenu).ChangeElementSelected(curNode.prev._item.GetComponent<NetworkedUnit>().gridElement.gameObject);
    }

    private DoublyLinkedListNode FindNode(NetworkedUnit unit)
    {
        DoublyLinkedListNode current = head;
        if (current == null) return null;
        do
        {
            if (current._item == unit)
                break;
            current = current.forward;
        } while (current.forward != head);
        return current;
    }

    [Command]
    public void CmdEndLevel(PlayerEnum identity)
    {
        gameManager.EndLevel(identity);
    }
}
