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
    public GridElement gridElement; // what grid element this piece is on
    protected ContextMenu contextMenu;
    protected string action = "";     // what action this piece is to perform; should we make this an enum?
    public UnitType unitType;

	// Use this for initialization
	protected virtual void Start()
    {
        contextMenu = GetComponent<ContextMenu>();
        grid = FindObjectOfType<GridMenu>();
        FindGridElement();
        unitType = UnitType.Unit;
	}

    public bool FindGridElement()
    {
        // set up raycast
        Ray ray;
        RaycastHit info;

        // Look towards the grid (+z direction)
        ray = new Ray(transform.position + new Vector3(0.5f, 0.5f, 0), Vector3.forward);
        Physics.Raycast(ray, out info);
        Debug.Log(info.collider.gameObject.name);
        if (info.collider != null)
        {
            GameObject foundGameObject = info.collider.gameObject;
            // look for a GridElement, indicating this is on the grid
            if (foundGameObject.GetComponent<GridElement>() != null)
            {
                gridElement = foundGameObject.GetComponent<GridElement>();
                gridElement.piece = gameObject;
                return true;
            }
        }
        return false;
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
        gridElement.DisplayMoveTiles(remainingMoves, GridMenu.moveColor, true);
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

    public void MoveUnit(GameObject newLoc)
    {
        // set the action to null
        action = "";
        // Get the distance to new element
        int distance = newLoc.GetComponent<GridElement>().distance;
        // Turn off the movement grid
        gridElement.DisplayMoveTiles(remainingMoves, Menu.defaultColor, false);
        // Set the remaining moves appropriately
        remainingMoves = distance;
        MoveUnitNoAction(newLoc);
    }

    public void MoveUnitNoAction(GameObject newLoc)
    {
        transform.position = new Vector3(newLoc.transform.position.x - 0.5f, newLoc.transform.parent.transform.position.y - 0.5f, transform.position.z);
        // Handle Collisions; We're assuming newLoc always has a GridElement
        GridElement otherGE = newLoc.GetComponent<GridElement>();
        if (otherGE && otherGE.piece)
        {
            // We can assume that this is a Unit because only Units modify gridElement.piece
            grid.gameMan.ReturnUnit(otherGE.piece);
            otherGE.piece = null;
            // Don't forget to kill yourself
            grid.gameMan.ReturnUnit(gameObject);
            gridElement.piece = null;
        }
        else
        {
            gridElement.piece = null;
            gridElement = newLoc.GetComponent<GridElement>();
            gridElement.piece = gameObject;
        }
    }

    public void HideMovementGrid()
    {
        gridElement.DisplayMoveTiles(remainingMoves, Menu.defaultColor, false);
        grid.activeGO = null;
        grid.SetElementColor(grid.selectedGO, Menu.selectedColor, Menu.defaultColor);
    }

}
