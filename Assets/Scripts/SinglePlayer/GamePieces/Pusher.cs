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

    public override void DisplayActionGrid()
    {
        if (canAct)
        {
            action = "push";
            //contextMenu.HideContextMenu();
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

    public override void HideAction()
    {
        base.HideAction();  // covers if action == move
        if (action == "push")
        {
            gridElement.DisplayPusherInfluence(false);
            canAct = true;
        }
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
            GridElement lastGridElement;
            if (pushLoc == gridElement.northNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 0);
                while(lastGridElement != gridElement)
                {
                    if (!lastGridElement.northWall && lastGridElement.northNeighbor)
                    {
                        if (lastGridElement.piece.GetComponent<GamePiece>() is Unit) lastGridElement.piece.GetComponent<Unit>().SetLocation(lastGridElement.northNeighbor);
                        else if (lastGridElement.piece.GetComponent<GamePiece>() is Trap) lastGridElement.piece.GetComponent<Trap>().SetLocation(lastGridElement.northNeighbor);

                    }
                    lastGridElement = lastGridElement.southNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.eastNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 1);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.eastWall && lastGridElement.eastNeighbor)
                    {
                        if (lastGridElement.piece.GetComponent<GamePiece>() is Unit) lastGridElement.piece.GetComponent<Unit>().SetLocation(lastGridElement.eastNeighbor);
                        else if (lastGridElement.piece.GetComponent<GamePiece>() is Trap) lastGridElement.piece.GetComponent<Trap>().SetLocation(lastGridElement.eastNeighbor);
                    }
                    lastGridElement = lastGridElement.westNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.southNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 2);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.southWall && lastGridElement.southNeighbor)
                    {
                        if (lastGridElement.piece.GetComponent<GamePiece>() is Unit) lastGridElement.piece.GetComponent<Unit>().SetLocation(lastGridElement.southNeighbor);
                        else if (lastGridElement.piece.GetComponent<GamePiece>() is Trap) lastGridElement.piece.GetComponent<Trap>().SetLocation(lastGridElement.southNeighbor);
                    }
                    lastGridElement = lastGridElement.northNeighbor.GetComponent<GridElement>();
                }
            }
            if (pushLoc == gridElement.westNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 3);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.westWall && lastGridElement.westNeighbor)
                    {
                        if (lastGridElement.piece.GetComponent<GamePiece>() is Unit) lastGridElement.piece.GetComponent<Unit>().SetLocation(lastGridElement.westNeighbor);
                        else if (lastGridElement.piece.GetComponent<GamePiece>() is Trap) lastGridElement.piece.GetComponent<Trap>().SetLocation(lastGridElement.westNeighbor);
                    }
                    lastGridElement = lastGridElement.eastNeighbor.GetComponent<GridElement>();
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
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit || neighborPiece && neighborPiece.GetComponent<GamePiece>() is Trap)
                        return FindLastPieceInChain(location.northNeighbor.GetComponent<GridElement>(), 0);
                }
                break;
            case 1:
                if (location.eastNeighbor && !location.eastWall)
                {
                    neighborPiece = location.eastNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit || neighborPiece && neighborPiece.GetComponent<GamePiece>() is Trap)
                        return FindLastPieceInChain(location.eastNeighbor.GetComponent<GridElement>(), 1);
                }
                break;
            case 2:
                if (location.southNeighbor && !location.southWall)
                {
                    neighborPiece = location.southNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit || neighborPiece && neighborPiece.GetComponent<GamePiece>() is Trap)
                        return FindLastPieceInChain(location.southNeighbor.GetComponent<GridElement>(), 2);
                }
                break;
            case 3:
                if (location.westNeighbor && !location.westWall)
                {
                    neighborPiece = location.westNeighbor.GetComponent<GridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<GamePiece>() is Unit || neighborPiece && neighborPiece.GetComponent<GamePiece>() is Trap)
                        return FindLastPieceInChain(location.westNeighbor.GetComponent<GridElement>(), 3);
                }
                break;
        }
        return location;
    }
}
