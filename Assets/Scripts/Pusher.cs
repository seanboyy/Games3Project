using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : Unit {
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivatePushButtion()
    {
        action = "push";
        gridElement.DisplayPusherInfluence(grid.pushColor);
    }

    public override void PerformAction(GameObject actionLocGO)
    {
        if(action == "move")
            base.PerformAction(actionLocGO);
        if(action == "push")
        {
            PushUnits();
        }
    }

    private void PushUnits()
    {

    }
}
