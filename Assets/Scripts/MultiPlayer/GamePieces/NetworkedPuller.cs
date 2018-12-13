using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPuller : NetworkedUnit
{
    protected override void OnEnable()
    {
        if (!initialized)
        {
            base.OnEnable();
            unitType = UnitType.Puller;
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
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.southNeighbor, location.gameObject);
                break;
            case 1:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.westNeighbor, location.gameObject);
                break;
            case 2:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.northNeighbor, location.gameObject);
                break;
            case 3:
                owner.GetComponent<NetworkedPlayer>().CmdMovePiece(location.eastNeighbor, location.gameObject);
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
