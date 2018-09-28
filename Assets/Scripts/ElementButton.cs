using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementButton : MonoBehaviour
{
    [Header("Neighboring UI Elements - Set Dynamically")]
    public GameObject northNeighbor;
    public GameObject eastNeighbor;
    public GameObject southNeighbor;
    public GameObject westNeighbor;


	// Use this for initialization
	void Start ()
    {
        FindNeighbors();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PrintButtonClick()
    {
        Debug.Log("ElementButton::PrintButtonClick() - " + gameObject.name + " was clicked");
    }

    // Find the neighboring UI elements dynamically through raycasts (won't find UI elements without a collider)
    public void FindNeighbors()
    {
        // clear out old neighbors
        eastNeighbor = null;
        westNeighbor = null;
        northNeighbor = null;
        southNeighbor = null;

        // set up raycast
        Ray ray;
        RaycastHit info;

        // Look to the right (east)
        ray = new Ray(transform.position, new Vector3(1, 0));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("ElementButton::FindNeighbors() - Neighbor found to the east! - " + info.collider.gameObject.name);
            eastNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("ElementButton::FindNeighbors() - No neighbor found to the east");
        }

        // look to the left (west)
        ray = new Ray(transform.position, new Vector3(-1, 0));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("ElementButton::FindNeighbors() - Neighbor found to the west! - " + info.collider.gameObject.name);
            westNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("ElementButton::FindNeighbors() - No neighbor found to the west");
        }

        // look to the up (north)
        ray = new Ray(transform.position, new Vector3(0, 1));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("ElementButton::FindNeighbors() - Neighbor found to the north! - " + info.collider.gameObject.name);
            northNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("ElementButton::FindNeighbors() - No neighbor found to the north");
        }    
            
        // look to the down (south)
        ray = new Ray(transform.position, new Vector3(0, -1));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("ElementButton::FindNeighbors() - Neighbor found to the south! - " + info.collider.gameObject.name);
            southNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("ElementButton::FindNeighbors() - No neighbor found to the south");
        }

    }
}
