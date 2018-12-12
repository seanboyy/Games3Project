using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedUnit : NetworkedGamePiece
{
    public const int MAX_NUM_MOVES = 2;
    public int remainingMoves = 2;

    public NetworkedGridMenu grid;
    protected string action = "";     // what action this piece is to perform; should we make this an enum?
    public UnitType unitType;
    public GameObject owner;
    [HideInInspector]
    public bool canAct = true;

    //[HideInInspector]
    public GameObject flag;            // a reference to the flag; only used if this unit has the flag

    // Use this for initialization
    protected virtual void Start()
    {
        grid = FindObjectOfType<NetworkedGridMenu>();
        //FindGridElement();
        unitType = UnitType.Unit;
        //owner.GetComponent<NetworkedPlayer>().ReturnUnit(gameObject);
        Debug.Log(name + netId);
    }

    // This function should do nothing in the generic Unit. It is implemented on the Puller, Pusher, and Twister
    public virtual void DisplayActionGrid() { }

    private void OnEnable()
    {
        owner = transform.root.gameObject;
    }

    public void DisplayMoveGrid()
    {
        // set the action to "move"
        action = "move";
        // Close the ContextMenu
        //contextMenu.HideContextMenu();
        // Show the Movement Grid
        gridElement.DisplayMoveTiles(remainingMoves, true);
        grid.SetElementColor(gridElement.gameObject, NetworkedGridMenu.activeColor);
        grid.activeGO = gridElement.gameObject;
    }

    public virtual void PerformAction(GameObject actionLocGO)
    {
        if (action == "move")
        {
            MoveUnit(actionLocGO);
        }
    }

    public virtual void HideAction()
    {
        grid.activeGO = null;
        grid.SetElementColor(grid.selectedGO, NetworkedMenu.selectedColor, NetworkedMenu.defaultColor);
        if (action == "move")
            HideMovementGrid();
    }

    public void MoveUnit(GameObject newLoc)
    {
        // set the action to null
        action = "";
        // Get the distance to new element
        int distance = newLoc.GetComponent<NetworkedGridElement>().distance;
        // Turn off the movement grid
        gridElement.DisplayMoveTiles(remainingMoves, false);
        // Set the remaining moves appropriately
        remainingMoves = distance;
        owner.GetComponent<NetworkedPlayer>().CmdMovePiece(newLoc, gridElement.gameObject);
    }

    public void SetLocation(GameObject newLoc)
    {
        transform.position = newLoc.transform.TransformPoint(Vector3.zero) + Vector3.forward * gameObject.transform.position.z;
        if (flag)
        {
            flag.transform.position = newLoc.transform.TransformPoint(Vector3.zero) + Vector3.forward * flag.transform.position.z;
            flag.GetComponent<NetworkedGamePiece>().gridElement = newLoc.GetComponent<NetworkedGridElement>();
            if (newLoc.GetComponent<NetworkedGridElement>().goal)
                grid.gameMan.EndLevel();
        }
        // Check if gridElement has been assigned (this is for spawning)
        if (!gridElement)
        {
            if (!FindGridElement())
                Debug.Log(gridElement.piece);
        }
        // Handle Collisions; We're assuming newLoc always has a GridElement
        NetworkedGridElement otherGE = newLoc.GetComponent<NetworkedGridElement>();
        if (otherGE && otherGE.piece && otherGE.piece != gameObject)
        {
            //Debug.Log("Collided with: " + otherGE.piece.name);
            // Check to make sure we're working with a unit
            if (otherGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedUnit)
            {
                NetworkedUnit otherUnit = otherGE.piece.GetComponent<NetworkedUnit>();
                otherUnit.owner.GetComponent<NetworkedPlayer>().CmdReturnUnit(otherGE.piece);
                if (otherUnit.unitType == UnitType.PortalPlacer)
                {
                    otherUnit.GetComponent<NetworkedPortalPlacer>().PlacePortal(otherGE);
                }
                otherGE.piece = null;
                // check to see if the other piece has the flag
                if (otherUnit.flag)
                {
                    otherGE.piece = otherUnit.flag;
                    flag = null;
                }
                // make sure you don't have the flag
                if (flag)
                {
                    otherGE.piece = flag;
                    flag.GetComponent<NetworkedGamePiece>().gridElement = otherGE;
                    flag = null;
                }
                // Don't forget to kill yourself
                owner.GetComponent<NetworkedPlayer>().CmdReturnUnit(gameObject);
                if (unitType == UnitType.PortalPlacer)
                    GetComponent<NetworkedPortalPlacer>().PlacePortal(otherGE);
                gridElement.piece = null;
                return;
            }
            else
            {
                // Check for flag
                if (otherGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedFlag)
                {
                    flag = otherGE.piece;
                    canAct = false;
                    remainingMoves = 0;
                }
                else if (otherGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedTrap)
                {
                    if (unitType == UnitType.PortalPlacer)
                        this.GetComponent<NetworkedPortalPlacer>().PlacePortal(otherGE);
                    if (gridElement.piece == gameObject) gridElement.piece = null;
                    if (!grid) grid = FindObjectOfType<NetworkedGridMenu>();
                    owner.GetComponent<NetworkedPlayer>().CmdReturnUnit(gameObject);
                    if (flag)   // flags will destroy traps; currently no piece can destroy traps, 
                                // so if a flag lands on one, it must either destroy the trap or the game is unwinnable
                                // it may be better to have traps pull/pushable, while the flag remains aloof. This would 
                                // prevent the need to destroy the trap
                    {
                        Destroy(otherGE.piece);
                        otherGE.piece = flag;
                        flag.GetComponent<NetworkedGamePiece>().gridElement = otherGE;
                        flag = null;
                    }
                    return;
                }
            }
        }
        gridElement.piece = null;
        gridElement = newLoc.GetComponent<NetworkedGridElement>();
        gridElement.piece = gameObject;

        if (flag)
        {
            flag.GetComponent<NetworkedGamePiece>().gridElement = gridElement;
            if (gridElement.goal)
            {
                // Player has moved the flag into the goal!
                // Someone should probably contact the gamemanager
                grid.gameMan.EndLevel();
            }
        }
    }

    public void HideMovementGrid()
    {
        gridElement.DisplayMoveTiles(remainingMoves, false);
    }

    public void ResetPiece()
    {
        if (!flag)
        {
            canAct = true;
        }
        remainingMoves = MAX_NUM_MOVES;
    }
}
