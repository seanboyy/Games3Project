using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : GamePiece
{
    public GridMenu grid;

    // Use this for initialization
    void Start()
    {
        FindGridElement();
        grid = FindObjectOfType<GridMenu>();
    }

    public void SetLocation(GameObject newLoc)
    {

        // Check if gridElement has been assigned (this is for spawning)
        if (!gridElement)
        {
            FindGridElement();
        }
        // Handle Collisions; We're assuming newLoc always has a GridElement
        GridElement otherGE = newLoc.GetComponent<GridElement>();
        if (otherGE && otherGE.piece && otherGE.piece != gameObject)
        {
            Debug.Log("Collided with: " + otherGE.piece.name);
            // Check to make sure we're working with a unit
            if (otherGE.piece.GetComponent<GamePiece>() is Unit)
            {
                Unit otherUnit = otherGE.piece.GetComponent<Unit>();
                grid.gameMan.ReturnUnit(otherGE.piece, otherUnit.owner);
                if (otherUnit is PortalPlacer)
                {
                    otherUnit.GetComponent<PortalPlacer>().PlacePortal(otherGE);
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
                if (otherGE.piece.GetComponent<GamePiece>() is Flag)
                {
                    Debug.Log("MAYDAY - A TRAP HAS LANDED ON THE FLAG AND THOMAS HASN'T FIGURED OUT HOW THIS SHOULD ACT.");
                }
                else if (otherGE.piece.GetComponent<GamePiece>() is Trap)
                {
                    Debug.Log("MAYDAY - A TRAP HAS LANDED ON A TRAP AND THOMAS HASN'T FIGURED OUT HOW THIS SHOULD ACT.");
                }
            }
        }
        transform.position = newLoc.transform.TransformPoint(Vector3.zero) + Vector3.forward * gameObject.transform.position.z;
        gridElement.piece = null;
        gridElement = newLoc.GetComponent<GridElement>();
        gridElement.piece = gameObject;
    }

}
