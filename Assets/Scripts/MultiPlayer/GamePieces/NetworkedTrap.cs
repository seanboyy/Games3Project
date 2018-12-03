using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedTrap : NetworkedGamePiece
{
    // Use this for initialization
    void Start()
    {
        FindGridElement();
    }

    public void SetLocation(GameObject newLoc)
    {

        // Check if gridElement has been assigned (this is for spawning)
        if (!gridElement)
        {
            FindGridElement();
        }
        // Handle Collisions; We're assuming newLoc always has a GridElement
        NetworkedGridElement otherGE = newLoc.GetComponent<NetworkedGridElement>();
        if (otherGE && otherGE.piece && otherGE.piece != gameObject)
        {
            //Debug.Log("Collided with: " + otherGE.piece.name);
            // Check to make sure we're working with a unit
            if (otherGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedUnit)
            {
                NetworkedUnit otherUnit = otherGE.piece.GetComponent<NetworkedUnit>();
                otherUnit.GetComponent<NetworkedPlayer>().ReturnUnit(otherGE.piece);
                if (otherUnit is NetworkedPortalPlacer)
                {
                    otherUnit.GetComponent<NetworkedPortalPlacer>().PlacePortal(otherGE);
                }
                otherGE.piece = null;
                // check to see if the other piece has the flag
                if (otherUnit.flag)
                {
                    otherGE.piece = otherUnit.flag;
                }
            }
            else
            {
                // Check for flag
                if (otherGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedFlag)
                {
                    Debug.Log("MAYDAY - A TRAP HAS LANDED ON THE FLAG AND THOMAS HASN'T FIGURED OUT HOW THIS SHOULD ACT.");
                }
                else if (otherGE.piece.GetComponent<NetworkedGamePiece>() is NetworkedTrap)
                {
                    Debug.Log("MAYDAY - A TRAP HAS LANDED ON A TRAP AND THOMAS HASN'T FIGURED OUT HOW THIS SHOULD ACT.");
                }
            }
        }
        transform.position = newLoc.transform.TransformPoint(Vector3.zero) + Vector3.forward * gameObject.transform.position.z;
        gridElement.piece = null;
        gridElement = newLoc.GetComponent<NetworkedGridElement>();
        gridElement.piece = gameObject;
    }

}
