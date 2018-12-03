using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedGamePiece : NetworkBehaviour
{
    public GameObject visualObject;

    public GameObject canvas;

    public NetworkedGridElement gridElement; // what grid element this piece is on

    protected string pieceName;

    public bool FindGridElement()
    {
        // set up raycast
        Ray ray;
        RaycastHit info;

        // Look towards the grid (+z direction)
        ray = new Ray(transform.position, Vector3.forward);
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            GameObject foundGameObject = info.collider.gameObject;
            // look for a GridElement, indicating this is on the grid
            if (foundGameObject.GetComponent<NetworkedGridElement>())
            {
                gridElement = foundGameObject.GetComponent<NetworkedGridElement>();
                if (gridElement.piece)
                {
                    // There is probably just a flag here
                    return false;
                }
                gridElement.piece = gameObject;
                return true;
            }
        }
        return false;
    }

}
