using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public GameObject visualObject;

    public GameObject canvas;

    public GridElement gridElement; // what grid element this piece is on

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
            if (foundGameObject.GetComponent<GridElement>())
            {
                gridElement = foundGameObject.GetComponent<GridElement>();
                if (gridElement.piece)
                {
                    Debug.Log("there's a piece here");
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
