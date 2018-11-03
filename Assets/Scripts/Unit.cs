using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitType
{
    Unit,
    Pusher,
    Puller,
    Twister,
    Flagbearer,
    PortalPlacer
}

public class Unit : GamePiece
{
    public const int MAX_NUM_MOVES = 2;
    public int remainingMoves = 2;

    public GridMenu grid;
    protected ContextMenu contextMenu;
    protected string action = "";     // what action this piece is to perform; should we make this an enum?
    public UnitType unitType;
    [HideInInspector]
    public bool canAct = true;

    //[HideInInspector]
    public GameObject flag;            // a reference to the flag; only used if this unit has the flag

    // Use this for initialization
    protected virtual void Start()
    {
        contextMenu = GetComponent<ContextMenu>();
        grid = FindObjectOfType<GridMenu>();
        //FindGridElement();
        unitType = UnitType.Unit;
    }

    public void ShowContextMenu()
    {
        contextMenu.ShowContextMenu(grid.gameObject);
    }

    public void HideContextMenu()
    {
        contextMenu.HideContextMenu();
    }

    public void ActivateMoveButton()
    {
        // set the action to "move"
        action = "move";
        // Close the ContextMenu
        contextMenu.HideContextMenu();
        // Show the Movement Grid
        gridElement.DisplayMoveTiles(remainingMoves, true);
        grid.SetElementColor(gridElement.gameObject, GridMenu.activeColor);
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
        grid.SetElementColor(grid.selectedGO, Menu.selectedColor, Menu.defaultColor);
        if (action == "move")
            HideMovementGrid();
    }

    public void MoveUnit(GameObject newLoc)
    {
        // set the action to null
        action = "";
        // Get the distance to new element
        int distance = newLoc.GetComponent<GridElement>().distance;
        // Turn off the movement grid
        gridElement.DisplayMoveTiles(remainingMoves, false);
        // Set the remaining moves appropriately
        remainingMoves = distance;
        SetLocation(newLoc);
    }

    public void SetLocation(GameObject newLoc)
    {
        transform.position = newLoc.transform.TransformPoint(Vector3.zero) + Vector3.forward * gameObject.transform.position.z;
        if (flag)
            flag.transform.position = newLoc.transform.TransformPoint(Vector3.zero) + Vector3.forward * flag.transform.position.z;
        // Check if gridElement has been assigned (this is for spawning)
        if (!gridElement)
        {
            FindGridElement();
        }
        // Handle Collisions; We're assuming newLoc always has a GridElement
        GridElement otherGE = newLoc.GetComponent<GridElement>();
        if (otherGE && otherGE.piece && otherGE.piece != gameObject)
        {
            Debug.Log("Collided with: " + otherGE.piece.name);
            // Check to make sure we're working with a unit
            if (otherGE.piece.GetComponent<GamePiece>() is Unit)
            {
                Unit otherUnit = otherGE.piece.GetComponent<Unit>();
                grid.gameMan.ReturnUnit(otherGE.piece);
                if (otherUnit.unitType == UnitType.PortalPlacer)
                {
                    if (grid.portalPlaced)
                    {
                        grid.portalLocation.portal = false;
                        grid.portalLocation.ChangeColor(Menu.defaultColor);
                    }
                    grid.portalLocation = otherGE;
                    grid.portalPlaced = true;
                    otherGE.portal = true;
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
                    flag.GetComponent<GamePiece>().gridElement = otherGE;
                    flag = null;
                }
                // Don't forget to kill yourself
                grid.gameMan.ReturnUnit(gameObject);
                if (unitType == UnitType.PortalPlacer)
                    otherGE.portal = true;
                gridElement.piece = null;
                return;
            }
            else    
            {
                // Check for flag
                if (otherGE.piece.GetComponent<GamePiece>() is Flag)
                {
                    flag = otherGE.piece;
                    canAct = false;
                    remainingMoves = 0;
                }
                else if (otherGE.piece.GetComponent<GamePiece>() is Trap)
                {
                    grid.gameMan.ReturnUnit(gameObject);
                    if (flag)   // flags will destroy traps; currently no piece can destroy traps, 
                                // so if a flag lands on one, it must either destroy the trap or the game is unwinnable
                                // it may be better to have traps pull/pushable, while the flag remains aloof. This would 
                                // prevent the need to destroy the trap
                    {
                        Destroy(otherGE.piece);
                        otherGE.piece = flag;
                        flag.GetComponent<GamePiece>().gridElement = otherGE;
                        flag = null;
                    }
                }
            }
        }
        gridElement.piece = null;
        gridElement = newLoc.GetComponent<GridElement>();
        gridElement.piece = gameObject;

        if (flag)
        {
            flag.GetComponent<GamePiece>().gridElement = gridElement;
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
