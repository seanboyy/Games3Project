﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puller : Unit
{
    protected override void Start()
    {
        base.Start();
        unitType = UnitType.Puller;
    }

    public override bool DisplayActionGrid()
    {
        if (canAct)
        {
            action = "pull";
            //contextMenu.HideContextMenu();
            gridElement.DisplayPullerInfluence(2, true);
            canAct = false;
        }
        return true;
    }

    public override void PerformAction(GameObject actionLocGO)
    {
        if (action == "move") base.PerformAction(actionLocGO);
        if (action == "pull")
            PullUnits(actionLocGO);
    }

    public override void HideAction()
    {
        base.HideAction();
        if(action == "pull")
        {
            gridElement.DisplayPullerInfluence(2, false);
            canAct = true;
        }
    }

    private void PullUnits(GameObject pullLoc)
    {
        action = "";
        gridElement.DisplayPullerInfluence(2, false);
        GridElement location = pullLoc.GetComponent<GridElement>();
        switch (FindDirection(pullLoc))
        {
            case 0:
                if (location.piece.GetComponent<GamePiece>() is Unit) location.piece.GetComponent<Unit>().SetLocation(location.southNeighbor);
                else if (location.piece.GetComponent<GamePiece>() is Trap) location.piece.GetComponent<Trap>().SetLocation(location.southNeighbor);
                break;
            case 1:
                if (location.piece.GetComponent<GamePiece>() is Unit) location.piece.GetComponent<Unit>().SetLocation(location.westNeighbor);
                else if (location.piece.GetComponent<GamePiece>() is Trap) location.piece.GetComponent<Trap>().SetLocation(location.westNeighbor);
                break;
            case 2:
                if (location.piece.GetComponent<GamePiece>() is Unit) location.piece.GetComponent<Unit>().SetLocation(location.northNeighbor);
                else if (location.piece.GetComponent<GamePiece>() is Trap) location.piece.GetComponent<Trap>().SetLocation(location.northNeighbor);
                break;
            case 3:
                if (location.piece.GetComponent<GamePiece>() is Unit) location.piece.GetComponent<Unit>().SetLocation(location.eastNeighbor);
                else if (location.piece.GetComponent<GamePiece>() is Trap) location.piece.GetComponent<Trap>().SetLocation(location.eastNeighbor);
                break;
        }
    }

    //Will return
    // 0 for north
    // 1 for east
    // 2 for south
    // 3 for west
    // -1 for ?
    private int FindDirection(GameObject location)
    {
        int ret = -1;
        if (location.transform.position.y > gridElement.transform.position.y) ret = 0;
        if (location.transform.position.x > gridElement.transform.position.x) ret = 1;
        if (location.transform.position.y < gridElement.transform.position.y) ret = 2;
        if (location.transform.position.x < gridElement.transform.position.x) ret = 3;
        return ret;
    }
}
