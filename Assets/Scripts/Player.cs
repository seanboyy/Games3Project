using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum PlayerEnum
{
    Player1,
    Player2,
    none
}

public class Player : NetworkBehaviour
{
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

    // Use this for initialization
    void Start()
    {
        FindGameManager();
        unitPool = new ObjectPool(unitPrefab, false, 1);
        pusherPool = new ObjectPool(pusherPrefab, false, 1);
        pullerPool = new ObjectPool(pullerPrefab, false, 1);
        twisterPool = new ObjectPool(twisterPrefab, false, 1);
        portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 1);
        if (gameManager == null)
            FindGameManager();
    }

    void FindGameManager()
    {
        if (FindObjectOfType<MultiMan>())
        {
            gameManager = FindObjectOfType<MultiMan>();
        }
        if (FindObjectOfType<SingleMan>())
        {
            gameManager = FindObjectOfType<SingleMan>();
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
        if (gameManager == null)
        {
            FindGameManager();
            return;
        }
        if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            gameManager.HandleHorizontalMovement(gameObject, Input.GetAxisRaw("Horizontal"));
        if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            gameManager.HandleVerticalMovement(gameObject, Input.GetAxisRaw("Vertical"));
        prevHorAxis = Input.GetAxisRaw("Horizontal");
        prevVerAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            if (gameManager is MultiMan) ((MultiMan)gameManager).CmdHandleCrossButton(isLocalPlayer, gameObject);
            else gameManager.HandleCrossButton(gameObject);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            if (gameManager is MultiMan) ((MultiMan)gameManager).CmdHandleCircleButton(isLocalPlayer, gameObject);
            else gameManager.HandleCircleButton(gameObject);
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            if (gameManager is MultiMan) ((MultiMan)gameManager).CmdHandleTriangleButton(isLocalPlayer, gameObject);
            else gameManager.HandleTriangleButton(gameObject);
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            if (gameManager is MultiMan) ((MultiMan)gameManager).CmdHandleSquareButton(isLocalPlayer, gameObject);
            else gameManager.HandleSquareButton(gameObject);
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
