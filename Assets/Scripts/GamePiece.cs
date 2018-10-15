using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public GameObject visualObject;

    public GameObject canvas;

    protected string pieceName;
    
    public int rotated;

    public void UpdateVisual()
    {
        switch(rotated % 4)
        {
            case 0:
                visualObject.transform.position += Vector3.right;
                canvas.transform.position += Vector3.right;
                break;
            case 1:
                visualObject.transform.position += Vector3.down;
                canvas.transform.position += Vector3.down;
                break;
            case 2:
                visualObject.transform.position += Vector3.left;
                canvas.transform.position += Vector3.left;
                break;
            case 3:
                visualObject.transform.position += Vector3.up;
                canvas.transform.position += Vector3.up;
                break;
        }
    }
}
