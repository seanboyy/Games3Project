using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPortalPlacer : NetworkedUnit
{

    private NetworkedGridElement portalElement;

    protected override void OnEnable()
    {
        if (!initialized)
        {
            base.OnEnable();
            unitType = UnitType.Portalist;
            //name = "" + unitType + owner.GetComponent<Player>().identity;
        }
        if (!grid)
        {
            grid = FindObjectOfType<NetworkedGridMenu>();
        }
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
