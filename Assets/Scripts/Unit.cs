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
        if (info.collider != null)
        {
            GameObject foundGameObject = info.collider.gameObject;
            // look for a GridElement, indicating this is on the grid
            if (foundGameObject.GetComponent<GridElement>())
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
        transform.position = newLoc.transform.TransformPoint(Vector3.zero) + new Vector3(-0.5F, -0.5F, gameObject.transform.position.z);
        // Handle Collisions; We're assuming newLoc always has a GridElement
        GridElement otherGE = newLoc.GetComponent<GridElement>();
        if (otherGE && otherGE.piece)
        {
            // We can assume that this is a Unit because only Units modify gridElement.piece
            grid.gameMan.ReturnUnit(otherGE.piece);
            if (otherGE.piece.GetComponent<Unit>().unitType == UnitType.PortalPlacer)
                otherGE.spawnable = true;
            otherGE.piece = null;
            // Don't forget to kill yourself
            grid.gameMan.ReturnUnit(gameObject);
            if (unitType == UnitType.PortalPlacer)
                otherGE.spawnable = true;
            gridElement.piece = null;
        }
        else
        {
            //if (!gridElement)
            //    gridElement = newLoc.GetComponent<GridElement>();
            gridElement.piece = null;
            gridElement = newLoc.GetComponent<GridElement>();
            gridElement.piece = gameObject;
        }
    }

    public void HideMovementGrid()
    {
        gridElement.DisplayMoveTiles(remainingMoves, false);
        grid.activeGO = null;
        grid.SetElementColor(grid.selectedGO, Menu.selectedColor, Menu.defaultColor);
    }

}
