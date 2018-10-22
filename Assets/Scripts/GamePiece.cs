using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public GameObject visualObject;

    public GameObject canvas;

    public GridElement gridElement; // what grid element this piece is on

    protected string pieceName;
    
    [HideInInspector]
    public int rotated;

    // used to fix rotating
    public void UpdateVisual()
    {
        //visualObject.transform.position += Vector3.down;
        //if (canvas) canvas.transform.position += Vector3.down;
        
        switch(rotated % 4)
        {
            case 0:
                Debug.Log("moving " + name + " right");
                visualObject.transform.position += Vector3.right;
                if(canvas) canvas.transform.position += Vector3.right;
                break;
            case 1:
                Debug.Log("moving " + name + " down");
                visualObject.transform.position += Vector3.down;
                if(canvas) canvas.transform.position += Vector3.down;
                break;
            case 2:
                Debug.Log("moving " + name + " left");
                visualObject.transform.position += Vector3.down;
                if (canvas) canvas.transform.position += Vector3.down;
                visualObject.transform.position += Vector3.left;
                if(canvas) canvas.transform.position += Vector3.left;
                break;
            case 3:
                Debug.Log("moving " + name + " up");
                visualObject.transform.position += Vector3.up;
                if(canvas) canvas.transform.position += Vector3.up;
                break;
        }
        //*/
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
