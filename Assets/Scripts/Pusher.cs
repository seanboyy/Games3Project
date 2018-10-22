using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : Unit
{
    protected override void Start()
    {
        base.Start();
        unitType = UnitType.Pusher;
    }

    public void ActivatePushButtion()
    {
        if (canAct)
        {
            action = "push";
            contextMenu.HideContextMenu();
            gridElement.DisplayPusherInfluence(true);
            canAct = false;
        }
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
        action = "";
        if (gridElement.northNeighbor == pushLoc
            || gridElement.eastNeighbor == pushLoc
            || gridElement.southNeighbor == pushLoc
            || gridElement.westNeighbor == pushLoc)
        {
            gridElement.DisplayPusherInfluence(false);
            GridElement lastPiece;
            if (pushLoc == gridElement.northNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 0);
                while(lastPiece != gridElement)
                {
                    if(!lastPiece.northWall && lastPiece.northNeighbor) lastPiece.piece.GetComponent<Unit>().SetLocation(lastPiece.northNeighbor);
                    lastPiece = lastPiece.southNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.eastNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 1);
                while (lastPiece != gridElement)
                {
                    if(!lastPiece.eastWall && lastPiece.eastNeighbor) lastPiece.piece.GetComponent<Unit>().SetLocation(lastPiece.eastNeighbor);
                    lastPiece = lastPiece.westNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.southNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 2);
                while (lastPiece != gridElement)
                {
                    if(!lastPiece.southWall && lastPiece.southNeighbor) lastPiece.piece.GetComponent<Unit>().SetLocation(lastPiece.southNeighbor);
                    lastPiece = lastPiece.northNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.westNeighbor)
            {
                lastPiece = FindLastPieceInChain(gridElement, 3);
                while (lastPiece != gridElement)
                {
                    if(!lastPiece.westWall && lastPiece.westNeighbor) lastPiece.piece.GetComponent<Unit>().SetLocation(lastPiece.westNeighbor);
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
                if (location.northNeighbor && ! location.northWall)
                {
                    neighborPiece = location.northNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                        return FindLastPieceInChain(location.northNeighbor.GetComponent<GridElement>(), 0);
                }
                break;
            case 1:
                if (location.eastNeighbor && !location.eastWall)
                {
                    neighborPiece = location.eastNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                        return FindLastPieceInChain(location.eastNeighbor.GetComponent<GridElement>(), 1);
                }
                break;
            case 2:
                if (location.southNeighbor && !location.southWall)
                {
                    neighborPiece = location.southNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                        return FindLastPieceInChain(location.southNeighbor.GetComponent<GridElement>(), 2);
                }
                break;
            case 3:
                if (location.westNeighbor && !location.westWall)
                {
                    neighborPiece = location.westNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit)
                        return FindLastPieceInChain(location.westNeighbor.GetComponent<GridElement>(), 3);
                }
                break;
        }
        return location;
    }
}
