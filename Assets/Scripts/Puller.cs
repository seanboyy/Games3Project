using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puller : Unit {
    public override void PerformAction(GameObject actionLocGO)
    {
        if (action == "move") base.PerformAction(actionLocGO);
        if (action == "pull")
            PullUnits(actionLocGO);
    }

    private void PullUnits(GameObject location)
    {

    }
}
