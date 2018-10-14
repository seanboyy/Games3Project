using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridElement : MonoBehaviour
{
    [Header("Can a piece spawn here?")]
    public bool spawnable = true; // Can a unit be spawned here?

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
            eastNeighbor = info.collider.gameObject;
        }

        // look to the left (west)
        ray = new Ray(transform.position, new Vector3(-1, 0));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            westNeighbor = info.collider.gameObject;
        }

        // look to the up (north)
        ray = new Ray(transform.position, new Vector3(0, 1));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            northNeighbor = info.collider.gameObject;
        }

        // look to the down (south)
        ray = new Ray(transform.position, new Vector3(0, -1));
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            southNeighbor = info.collider.gameObject;
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

    public void DisplayPusherInfluence(Color tileColor, bool shouldHighlight)
    {
        DisplayPusherInfluenceNorth(tileColor, shouldHighlight);
        DisplayPusherInfluenceEast(tileColor, shouldHighlight);
        DisplayPusherInfluenceSouth(tileColor, shouldHighlight);
        DisplayPusherInfluenceWest(tileColor, shouldHighlight);
    }

    private void DisplayPusherInfluenceNorth(Color tileColor, bool shouldHighlight)
    {
        if (northNeighbor && northNeighbor.GetComponent<GridElement>().piece && (northNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            northNeighbor.GetComponent<GridElement>().isHighlighted = shouldHighlight;
            northNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            northNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceNorth(tileColor, shouldHighlight);
        }
    }

    private void DisplayPusherInfluenceEast(Color tileColor, bool shouldHighlight)
    {
        if (eastNeighbor && eastNeighbor.GetComponent<GridElement>().piece && (eastNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            eastNeighbor.GetComponent<GridElement>().isHighlighted = shouldHighlight;
            eastNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            eastNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceEast(tileColor, shouldHighlight);
        }
    }

    private void DisplayPusherInfluenceSouth(Color tileColor, bool shouldHighlight)
    {
        if (southNeighbor && southNeighbor.GetComponent<GridElement>().piece && (southNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            southNeighbor.GetComponent<GridElement>().isHighlighted = shouldHighlight;
            southNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            southNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceSouth(tileColor, shouldHighlight);
        }
    }

    private void DisplayPusherInfluenceWest(Color tileColor, bool shouldHighlight)
    {
        if (westNeighbor && westNeighbor.GetComponent<GridElement>().piece && (westNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            westNeighbor.GetComponent<GridElement>().isHighlighted = shouldHighlight;
            westNeighbor.GetComponent<GridElement>().GetComponent<Image>().color = tileColor;
            westNeighbor.GetComponent<GridElement>().DisplayPusherInfluenceWest(tileColor, shouldHighlight);
        }
    }
}
