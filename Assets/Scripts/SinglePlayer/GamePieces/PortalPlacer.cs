using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPlacer : Unit {

    private GridElement portalElement;

    protected override void Start()
    {
        base.Start();
        unitType = UnitType.Portalist;
    }

    public void PlacePortal(GridElement newPortalLoc)
    {
        if (portalElement)
        {
            portalElement.portal = false;
            portalElement.portalOwner = PlayerEnum.none;
            portalElement.ChangeColor(Menu.defaultColor);
        }
        portalElement = newPortalLoc;
        // Check to make sure enemy portal isn't there
        if (portalElement.portal && portalElement.owner != owner.GetComponent<Player>().identity)
        {
            // if there is one, destory it
            portalElement.portal = false;
            portalElement.portalOwner = PlayerEnum.none;
            return;
        }
        portalElement.portal = true;
        portalElement.portalOwner = owner.GetComponent<Player>().identity;
    }
}
