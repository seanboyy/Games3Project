﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : Unit
{

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivatePushButtion()
    {
        action = "push";
        contextMenu.HideContextMenu();
        gridElement.DisplayPusherInfluence(grid.pushColor, true);
    }

    public override void PerformAction(GameObject actionLocGO)
    {
        if (action == "move")
            base.PerformAction(actionLocGO);
        if (action == "push")
            PushUnits(actionLocGO);
    }

    private void PushUnits(GameObject pushLoc)
    {
        Debug.Log("Trying to push units starting at " + pushLoc.name);
        action = "";
        if (gridElement.northNeighbor == pushLoc
            || gridElement.eastNeighbor == pushLoc
            || gridElement.southNeighbor == pushLoc
            || gridElement.westNeighbor == pushLoc)
        {
            gridElement.DisplayPusherInfluence(grid.defaultColor, false);
            GridElement lastPiece;
            if (pushLoc == gridElement.northNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 0);
                while(lastPiece != gridElement)
                {
                    lastPiece.piece.GetComponent<Unit>().MoveUnitNoAction(lastPiece.northNeighbor);
                    lastPiece = lastPiece.southNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.eastNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 1);
                while (lastPiece != gridElement)
                {
                    lastPiece.piece.GetComponent<Unit>().MoveUnitNoAction(lastPiece.eastNeighbor);
                    lastPiece = lastPiece.westNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.southNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 2);
                while (lastPiece != gridElement)
                {
                    lastPiece.piece.GetComponent<Unit>().MoveUnitNoAction(lastPiece.southNeighbor);
                    lastPiece = lastPiece.northNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.westNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 3);
                while (lastPiece != gridElement)
                {
                    lastPiece.piece.GetComponent<Unit>().MoveUnitNoAction(lastPiece.westNeighbor);
                    lastPiece = lastPiece.eastNeighbor.GetComponent<GridElement>();
                }
            }
        }
    }

    // Direction will be
    // 0 = north
    // 1 = east
    // 2 = south
    // 3 = west
    private GridElement FindLastPieceInChain(GridElement location, int direction)
    {
        GameObject neighborPiece;
        switch (direction)
        {
            case 0:
                neighborPiece = location.northNeighbor.GetComponent<GridElement>().piece;
                if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                    return FindLastPieceInChain(location.northNeighbor.GetComponent<GridElement>(), 0);
                break;
            case 1:
                neighborPiece = location.eastNeighbor.GetComponent<GridElement>().piece;
                if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                    return FindLastPieceInChain(location.eastNeighbor.GetComponent<GridElement>(), 1);
                break;
            case 2:
                neighborPiece = location.southNeighbor.GetComponent<GridElement>().piece;
                if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                    return FindLastPieceInChain(location.southNeighbor.GetComponent<GridElement>(), 2);
                break;
            case 3:
                neighborPiece = location.westNeighbor.GetComponent<GridElement>().piece;
                if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                    return FindLastPieceInChain(location.westNeighbor.GetComponent<GridElement>(), 3);
                break;
        }
        Debug.Log("Last piece in chain is at: " + location.name);
        return location;
    }
}