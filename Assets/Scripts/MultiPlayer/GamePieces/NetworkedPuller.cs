using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPuller : NetworkedUnit
{
    protected override void Start()
    {
        base.Start();
        unitType = UnitType.Puller;
    }

    public void ActivatePullButton()
    {
        if (canAct)
        {
            action = "pull";
            contextMenu.HideContextMenu();
            gridElement.DisplayPullerInfluence(2, true);
            canAct = false;
        }
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
        if (action == "pull")
        {
            gridElement.DisplayPullerInfluence(2, false);
            canAct = true;
        }
    }

    private void PullUnits(GameObject pullLoc)
    {
        action = "";
        gridElement.DisplayPullerInfluence(2, false);
        NetworkedGridElement location = pullLoc.GetComponent<NetworkedGridElement>();
        switch (FindDirection(pullLoc))
        {
            case 0:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.southNeighbor, location.piece);
                break;
            case 1:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.westNeighbor, location.piece);
                break;
            case 2:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.northNeighbor, location.piece);
                break;
            case 3:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.eastNeighbor, location.piece);
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
