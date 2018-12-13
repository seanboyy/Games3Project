using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPusher : NetworkedUnit
{
    protected override void OnEnable()
    {
        if (!initialized)
        {
            base.OnEnable();
            unitType = UnitType.Pusher;
            //name = "" + unitType + owner.GetComponent<Player>().identity;
        }
        if (!grid)
        {
            grid = FindObjectOfType<NetworkedGridMenu>();
        }
    }

    public override bool DisplayActionGrid()
    {
        if (canAct)
        {
            action = "push";
            //contextMenu.HideContextMenu();
            gridElement.DisplayPusherInfluence(true);
            canAct = false;
        }
        return true;
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
            NetworkedGridElement lastGridElement;
            if (pushLoc == gridElement.northNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 0);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.northWall && lastGridElement.northNeighbor)
                    {
                        owner.GetComponent<NetworkedPlayer>().CmdMovePiece(lastGridElement.northNeighbor, lastGridElement.gameObject);

                    }
                    lastGridElement = lastGridElement.southNeighbor.GetComponent<NetworkedGridElement>();
                }
            }
            if (pushLoc == gridElement.eastNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 1);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.eastWall && lastGridElement.eastNeighbor)
                    {
                        owner.GetComponent<NetworkedPlayer>().CmdMovePiece(lastGridElement.eastNeighbor, lastGridElement.gameObject);
                    }
                    lastGridElement = lastGridElement.westNeighbor.GetComponent<NetworkedGridElement>();
                }
            }
            if (pushLoc == gridElement.southNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 2);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.southWall && lastGridElement.southNeighbor)
                    {
                        owner.GetComponent<NetworkedPlayer>().CmdMovePiece(lastGridElement.southNeighbor, lastGridElement.gameObject);
                    }
                    lastGridElement = lastGridElement.northNeighbor.GetComponent<NetworkedGridElement>();
                }
            }
            if (pushLoc == gridElement.westNeighbor)
            {
                lastGridElement = FindLastPieceInChain(gridElement, 3);
                while (lastGridElement != gridElement)
                {
                    if (!lastGridElement.westWall && lastGridElement.westNeighbor)
                    {
                        owner.GetComponent<NetworkedPlayer>().CmdMovePiece(lastGridElement.westNeighbor, lastGridElement.gameObject);
                    }
                    lastGridElement = lastGridElement.eastNeighbor.GetComponent<NetworkedGridElement>();
                }
            }
        }
    }

    // Direction will be
    // 0 = north
    // 1 = east
    // 2 = south
    // 3 = west
    private NetworkedGridElement FindLastPieceInChain(NetworkedGridElement location, int direction)
    {
        GameObject neighborPiece;
        switch (direction)
        {
            case 0:
                if (location.northNeighbor && !location.northWall)
                {
                    neighborPiece = location.northNeighbor.GetComponent<NetworkedGridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedUnit || neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedTrap)
                        return FindLastPieceInChain(location.northNeighbor.GetComponent<NetworkedGridElement>(), 0);
                }
                break;
            case 1:
                if (location.eastNeighbor && !location.eastWall)
                {
                    neighborPiece = location.eastNeighbor.GetComponent<NetworkedGridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedUnit || neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedTrap)
                        return FindLastPieceInChain(location.eastNeighbor.GetComponent<NetworkedGridElement>(), 1);
                }
                break;
            case 2:
                if (location.southNeighbor && !location.southWall)
                {
                    neighborPiece = location.southNeighbor.GetComponent<NetworkedGridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedUnit || neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedTrap)
                        return FindLastPieceInChain(location.southNeighbor.GetComponent<NetworkedGridElement>(), 2);
                }
                break;
            case 3:
                if (location.westNeighbor && !location.westWall)
                {
                    neighborPiece = location.westNeighbor.GetComponent<NetworkedGridElement>().piece;
                    if (neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedUnit || neighborPiece && neighborPiece.GetComponent<NetworkedGamePiece>() is NetworkedTrap)
                        return FindLastPieceInChain(location.westNeighbor.GetComponent<NetworkedGridElement>(), 3);
                }
                break;
        }
        return location;
    }
}
