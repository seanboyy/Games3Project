using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPlacer : Unit {
    protected override void Start()
    {
        base.Start();
        unitType = UnitType.PortalPlacer;
    }
}
