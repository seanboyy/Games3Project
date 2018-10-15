using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagbearer : Unit {

    public bool hasFlag;

    public override void PerformAction(GameObject actionLocGO)
    {
        if(action == "move" && !hasFlag) base.PerformAction(actionLocGO);
        if (action == "move" && hasFlag) MoveUnitWithFlag(actionLocGO);
    }

    public void MoveUnitWithFlag(GameObject moveLoc)
    {

    }
}
