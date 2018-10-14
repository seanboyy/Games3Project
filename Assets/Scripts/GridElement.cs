using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridElement : MonoBehaviour
{
    [Header("Wall Placements")]
    public bool northWall = false;
    public bool eastWall = false;
    public bool southWall = false;
    public bool westWall = false;

    [Header("Neighboring UI Elements - Set Dynamically")]
    public GameObject northNeighbor;
    public GameObject eastNeighbor;
    public GameObject southNeighbor;
    public GameObject westNeighbor;

    [Header("Set Dynamically")]
    public bool isHighlighted = false;
    public int distance = -1;
    public GameObject piece;

    // Use this for initialization
    void Start()
    {
        FindNeighbors();
    }

    public void PrintButtonClick()
    {
        Debug.Log("GridElement::PrintButtonClick() - " + gameObject.name + " was clicked");
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
            //Debug.Log("GridElement::FindNeighbors() - Neighbor found to the east! - " + info.collider.gameObject.name);
            eastNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("GridElement::FindNeighbors() - No neighbor found to the east");
        }

        // look to the left (west)
        ray = new Ray(transform.position, new Vector3(-1, 0));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("GridElement::FindNeighbors() - Neighbor found to the west! - " + info.collider.gameObject.name);
            westNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("GridElement::FindNeighbors() - No neighbor found to the west");
        }

        // look to the up (north)
        ray = new Ray(transform.position, new Vector3(0, 1));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("GridElement::FindNeighbors() - Neighbor found to the north! - " + info.collider.gameObject.name);
            northNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("GridElement::FindNeighbors() - No neighbor found to the north");
        }

        // look to the down (south)
        ray = new Ray(transform.position, new Vector3(0, -1));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            //Debug.Log("GridElement::FindNeighbors() - Neighbor found to the south! - " + info.collider.gameObject.name);
            southNeighbor = info.collider.gameObject;
        }
        else
        {
            //Debug.Log("GridElement::FindNeighbors() - No neighbor found to the south");
        }
    }

    // Change the color of all tiles that can be accessed in movesRemaining moves from the current tile. 
    // Recursive
    public void DisplayMoveTiles(int movesRemaining, Color tileColor, bool showingMoves)
    {
        GetComponent<Image>().color = tileColor;
        isHighlighted = showingMoves;
        if (showingMoves)
            distance = movesRemaining;
        else
            distance = -1;
        if (movesRemaining <= 0)
            return;
        if (!eastWall && eastNeighbor != null)
            eastNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, tileColor, showingMoves);
        if (!westWall && westNeighbor != null)
            westNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, tileColor, showingMoves);
        if (!northWall && northNeighbor != null)
            northNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, tileColor, showingMoves);
        if (!southWall && southNeighbor != null)
            southNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, tileColor, showingMoves);
    }

    public void DisplayPusherInfluence(Color tileColor)
    {
        DisplayPusherInfluenceAll(tileColor);
    }

    private void DisplayPusherInfluenceAll(Color tileColor)
    {
        DisplayPusherInfluenceNorth(tileColor);
        DisplayPusherInfluenceEast(tileColor);
        DisplayPusherInfluenceSouth(tileColor);
        DisplayPusherInfluenceWest(tileColor);
    }

    private void DisplayPusherInfluenceNorth(Color tileColor)
    {

        if (northNeighbor && northNeighbor.GetComponent<GridElement>().piece)
        {
            northNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            northNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceNorth(tileColor);
        }
    }

    private void DisplayPusherInfluenceEast(Color tileColor)
    {

        if (eastNeighbor && eastNeighbor.GetComponent<GridElement>().piece)
        {
            eastNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            eastNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceEast(tileColor);
        }
    }

    private void DisplayPusherInfluenceSouth(Color tileColor)
    {

        if (southNeighbor && southNeighbor.GetComponent<GridElement>().piece)
        {
            southNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            southNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceSouth(tileColor);
        }
    }

    private void DisplayPusherInfluenceWest(Color tileColor)
    {

        if (westNeighbor && westNeighbor.GetComponent<GridElement>().piece)
        {
            westNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            westNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceWest(tileColor);
        }
    }
}
