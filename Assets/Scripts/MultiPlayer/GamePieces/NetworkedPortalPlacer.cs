using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPortalPlacer : NetworkedUnit
{

    private NetworkedGridElement portalElement;

    protected override void Start()
    {
        base.Start();
        unitType = UnitType.PortalPlacer;
    }

    public void PlacePortal(NetworkedGridElement newPortalLoc)
    {
        if (portalElement)
        {
            portalElement.portal = false;
            portalElement.portalOwner = PlayerEnum.none;
            portalElement.ChangeColor(NetworkedMenu.defaultColor);
        }
        portalElement = newPortalLoc;
        // Check to make sure enemy portal isn't there
        if (portalElement.portal && portalElement.owner != owner.GetComponent<NetworkedPlayer>().identity)
        {
            // if there is one, destory it
            portalElement.portal = false;
            portalElement.portalOwner = PlayerEnum.none;
            return;
        }
        portalElement.portal = true;
        portalElement.portalOwner = owner.GetComponent<NetworkedPlayer>().identity;
    }
}
