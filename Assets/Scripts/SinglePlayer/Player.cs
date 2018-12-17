using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    public PlayerEnum identity = PlayerEnum.Player1;
    public SingleMan gameManager;

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
    private DoublyLinkedListNode head;

    // These are SyncVars so the menu stays the same across client/server
    public Menu activeMenu;
    public Menu prevMenu;

    private GridMenu grid;

    // Use this for initialization
    void Start()
    {
        unitPool = new ObjectPool(unitPrefab, false, 1, transform);
        pusherPool = new ObjectPool(pusherPrefab, false, 1, transform);
        pullerPool = new ObjectPool(pullerPrefab, false, 1, transform);
        twisterPool = new ObjectPool(twisterPrefab, false, 1, transform);
        portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 1, transform);
        grid = FindObjectOfType<GridMenu>();
    }


    void Update()
    {
        if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            activeMenu.HandleHorizontalMovement(Input.GetAxisRaw("Horizontal"));
        if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            activeMenu.HandleVerticalMovement(Input.GetAxisRaw("Vertical"));
        prevHorAxis = Input.GetAxisRaw("Horizontal");
        prevVerAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            activeMenu.HandleCrossButton();
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            activeMenu.HandleCircleButton();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            gameManager.EndTurn();
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            activeMenu.HandleSquareButton();
        if (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.JoystickButton4))
            activeMenu.HandleLeftShoulderBumper();
        if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.JoystickButton5))
            activeMenu.HandleRightShoulderBumper();
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
            case UnitType.Portalist:
                unitGO = portalPlacerPool.GetObject();
                break;
        }
        if (unitGO)
        {
            Unit uScript = unitGO.GetComponent<Unit>();
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
            if (grid == null)
                grid = FindObjectOfType<GridMenu>();
            grid.UpdateDescription();
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
            case UnitType.Portalist:
                portalPlacerPool.ReturnObject(unit);
                break;
        }
        DoublyLinkedListNode curNode = FindNode(unit.GetComponent<Unit>());
        curNode.RemoveItem();
    }

    // This get's called by ContextMenu
    public void SetActiveMenu(Menu newMenu)
    {
        prevMenu = activeMenu;
        activeMenu = newMenu;
    }

    public void RotateLeft(GridElement selectedGE)
    {
        DoublyLinkedListNode curNode = head;
        if (selectedGE.piece && selectedGE.piece.GetComponent<GamePiece>() is Unit)
        {
            Unit selectedUnit = selectedGE.piece.GetComponent<Unit>();
            curNode = FindNode(selectedUnit);
        }

        if (activeMenu is GridMenu)
            ((GridMenu)activeMenu).ChangeElementSelected(curNode.forward.item.GetComponent<Unit>().gridElement.gameObject);
    }

    public void RotateRight(GridElement selectedGE)
    {
        DoublyLinkedListNode curNode = head;
        if (selectedGE.piece && selectedGE.piece.GetComponent<GamePiece>() is Unit)
        {
            Unit selectedUnit = selectedGE.piece.GetComponent<Unit>();
            curNode = FindNode(selectedUnit);
        }

        if (activeMenu is GridMenu)
            ((GridMenu)activeMenu).ChangeElementSelected(curNode.prev.item.GetComponent<Unit>().gridElement.gameObject);
    }

    private DoublyLinkedListNode FindNode(Unit unit)
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
