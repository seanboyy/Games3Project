﻿using System.Collections;
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

    [HideInInspector]
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
        visualObject.transform.localPosition = (Vector3.one - Vector3.forward) / 2F;
        canvas.transform.localPosition = Vector3.back * 2F;
        if (flag)
            flag.transform.position = newLoc.transform.TransformPoint(Vector3.zero) + new Vector3(-0.5F, -0.5F, flag.transform.position.z);
        // Handle Collisions; We're assuming newLoc always has a GridElement
        GridElement otherGE = newLoc.GetComponent<GridElement>();
        if (otherGE && otherGE.piece)
        {
            Debug.Log(otherGE.name);
            Debug.Log(otherGE.piece.name);
            // Check to make sure we're working with a unit
            if (otherGE.piece.GetComponent<GamePiece>() is Unit)
            {
                Debug.Log("Collided with non-Flag unit");
                Unit otherUnit = otherGE.piece.GetComponent<Unit>();
                grid.gameMan.ReturnUnit(otherGE.piece);
                if (otherUnit.unitType == UnitType.PortalPlacer)
                {
                    if (grid.portalPlaced)
                        grid.portalLocation.portal = false;
                    grid.portalLocation = otherGE;
                    grid.portalPlaced = true;
                    otherGE.portal = true;
                }
                otherGE.piece = null;
                // check to see if the other piece has the flag
                if (otherUnit.flag)
                    otherGE.piece = otherUnit.flag;
                // make sure you don't have the flag
                if (flag)
                {
                    otherGE.piece = flag;
                    flag.GetComponent<GamePiece>().gridElement = otherGE;
                }
                // Don't forget to kill yourself
                grid.gameMan.ReturnUnit(gameObject);
                if (unitType == UnitType.PortalPlacer)
                    otherGE.portal = true;
                gridElement.piece = null;
                return;
            }
            else    // assume it's the flag
            {
                flag = otherGE.piece;
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
        grid.activeGO = null;
        grid.SetElementColor(grid.selectedGO, Menu.selectedColor, Menu.defaultColor);
    }

}
