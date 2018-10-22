using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public GameObject visualObject;

    public GameObject canvas;

    public GridElement gridElement; // what grid element this piece is on

    protected string pieceName;
    
    public int rotated;

    // used to fix rotating
    public void UpdateVisual()
    {
        switch(rotated % 4)
        {
            case 0:
                visualObject.transform.position += Vector3.right;
                if(canvas) canvas.transform.position += Vector3.right;
                break;
            case 1:
                visualObject.transform.position += Vector3.down;
                if(canvas) canvas.transform.position += Vector3.down;
                break;
            case 2:
                visualObject.transform.position += Vector3.left;
                if(canvas) canvas.transform.position += Vector3.left;
                break;
            case 3:
                visualObject.transform.position += Vector3.up;
                if(canvas) canvas.transform.position += Vector3.up;
                break;
        }
    }

    public bool FindGridElement()
    {
        // set up raycast
        Ray ray;
        RaycastHit info;

        // Look towards the grid (+z direction)
        ray = new Ray(transform.position + new Vector3(0.5f, 0.5f, 0), Vector3.forward);
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            GameObject foundGameObject = info.collider.gameObject;
            // look for a GridElement, indicating this is on the grid
            if (foundGameObject.GetComponent<GridElement>())
            {
                gridElement = foundGameObject.GetComponent<GridElement>();
                gridElement.piece = gameObject;
                return true;
            }
        }
        return false;
    }

}
