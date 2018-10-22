using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagbearer : Unit
{

    public GameObject flag;

    public override void PerformAction(GameObject actionLocGO)
    {
        if (action == "move" && !flag) base.PerformAction(actionLocGO);
        if (action == "move" && flag) MoveUnitWithFlag(actionLocGO);
    }

    public void MoveUnitWithFlag(GameObject moveLoc)
    {
        MoveUnit(moveLoc);
    }
}