﻿using System.Collections;
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

    //private GridMenu grid;

    // Use this for initialization
    void Start()
    {
        //grid = FindObjectOfType<GridMenu>();
        FindNeighbors();
        UpdateWalls();
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

    public void UpdateWalls()
    {
        UpdateWallSprite(gameObject);
        if (northWall && northNeighbor)
        {
            northNeighbor.GetComponent<GridElement>().southWall = true;
            UpdateWallSprite(northNeighbor);
        }
        if (eastWall && eastNeighbor)
        {
            eastNeighbor.GetComponent<GridElement>().westWall = true;
            UpdateWallSprite(eastNeighbor);
        }
        if (southWall && southNeighbor)
        {
            southNeighbor.GetComponent<GridElement>().northWall = true;
            UpdateWallSprite(southNeighbor);
        }
        if (westWall && westNeighbor)
        {
            westNeighbor.GetComponent<GridElement>().eastWall = true;
            UpdateWallSprite(westNeighbor);
        }
    }

    public static void UpdateWallSprite(GameObject GO)
    {
        GridMenu grid = FindObjectOfType<GridMenu>();
        GridElement element = GO.GetComponent<GridElement>();
        if (element.northWall && element.eastWall && element.southWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.FourWalls;
        }
        else if (element.northWall && element.eastWall && element.southWall)
        {
            GO.GetComponent<Image>().sprite = grid.ThreeWalls;
            GO.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (element.northWall && element.eastWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.ThreeWalls;
        }
        else if (element.northWall && element.southWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.ThreeWalls;
            GO.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (element.eastWall && element.southWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.ThreeWalls;
            GO.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (element.northWall && element.eastWall)
        {
            GO.GetComponent<Image>().sprite = grid.TwoWallsL;
            GO.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (element.northWall && element.southWall)
        {
            GO.GetComponent<Image>().sprite = grid.TwoWallsHall;
            GO.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (element.northWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.TwoWallsL;
        }
        else if (element.eastWall && element.southWall)
        {
            GO.GetComponent<Image>().sprite = grid.TwoWallsL;
            GO.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (element.eastWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.TwoWallsHall;
        }
        else if (element.southWall && element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.TwoWallsL;
            GO.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (element.northWall)
        {
            GO.GetComponent<Image>().sprite = grid.OneWall;
            GO.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (element.eastWall)
        {
            GO.GetComponent<Image>().sprite = grid.OneWall;
            GO.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (element.southWall)
        {
            GO.GetComponent<Image>().sprite = grid.OneWall;
            GO.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (element.westWall)
        {
            GO.GetComponent<Image>().sprite = grid.OneWall;
        }
        else
        {
            GO.GetComponent<Image>().sprite = grid.NoWalls;
            GO.transform.rotation = Quaternion.identity;
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

    #region Pusher Influence
    public void DisplayPusherInfluence(Color tileColor, bool shouldHighlight)
    {
        DisplayPusherInfluenceNorth(tileColor, shouldHighlight);
        DisplayPusherInfluenceEast(tileColor, shouldHighlight);
        DisplayPusherInfluenceSouth(tileColor, shouldHighlight);
        DisplayPusherInfluenceWest(tileColor, shouldHighlight);
    }

    private void DisplayPusherInfluenceNorth(Color tileColor, bool shouldHighlight)
    {
        if (!northWall && northNeighbor && northNeighbor.GetComponent<GridElement>().piece && (northNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            GridElement north = northNeighbor.GetComponent<GridElement>();
            north.isHighlighted = shouldHighlight;
            north.GetComponent<Image>().color = tileColor;
            north.DisplayPusherInfluenceNorth(tileColor, shouldHighlight);
        }
    }

    private void DisplayPusherInfluenceEast(Color tileColor, bool shouldHighlight)
    {
        if (!eastWall && eastNeighbor && eastNeighbor.GetComponent<GridElement>().piece && (eastNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            GridElement east = eastNeighbor.GetComponent<GridElement>();
            east.isHighlighted = shouldHighlight;
            east.GetComponent<Image>().color = tileColor;
            east.DisplayPusherInfluenceEast(tileColor, shouldHighlight);
        }
    }

    private void DisplayPusherInfluenceSouth(Color tileColor, bool shouldHighlight)
    {
        if (!southWall && southNeighbor && southNeighbor.GetComponent<GridElement>().piece && (southNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            GridElement south = southNeighbor.GetComponent<GridElement>();
            south.isHighlighted = shouldHighlight;
            south.GetComponent<Image>().color = tileColor;
            south.DisplayPusherInfluenceSouth(tileColor, shouldHighlight);
        }
    }

    private void DisplayPusherInfluenceWest(Color tileColor, bool shouldHighlight)
    {
        if (!westWall && westNeighbor && westNeighbor.GetComponent<GridElement>().piece && (westNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit))
        {
            GridElement west = westNeighbor.GetComponent<GridElement>();
            west.isHighlighted = shouldHighlight;
            west.GetComponent<Image>().color = tileColor;
            west.DisplayPusherInfluenceWest(tileColor, shouldHighlight);
        }
    }
    #endregion

    #region Puller Influence
    public void DisplayPullerInfluence(int distance, Color tileColor, bool shouldHighlight)
    {
        DisplayPullerInfluenceNorth(distance, tileColor, shouldHighlight);
        DisplayPullerInfluenceEast(distance, tileColor, shouldHighlight);
        DisplayPullerInfluenceSouth(distance, tileColor, shouldHighlight);
        DisplayPullerInfluenceWest(distance, tileColor, shouldHighlight);
    }

    private void DisplayPullerInfluenceNorth(int distance, Color tileColor, bool shouldHighlight)
    {
        if (!northWall && northNeighbor && distance > 0)
        {
            GridElement north = northNeighbor.GetComponent<GridElement>();
            north.GetComponent<Image>().color = tileColor;
            if (north.piece && north.piece.GetComponent<GamePiece>() is Unit) north.isHighlighted = shouldHighlight;
            else north.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            north.DisplayPullerInfluenceNorth(distance - 1, tileColor, shouldHighlight);
        }
    }

    private void DisplayPullerInfluenceEast(int distance, Color tileColor, bool shouldHighlight)
    {
        if (!eastWall && eastNeighbor && distance > 0)
        {
            GridElement east = eastNeighbor.GetComponent<GridElement>();
            east.GetComponent<Image>().color = tileColor;
            if (east.piece && east.piece.GetComponent<GamePiece>() is Unit) east.isHighlighted = shouldHighlight;
            else east.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            east.DisplayPullerInfluenceEast(distance - 1, tileColor, shouldHighlight);
        }
    }

    private void DisplayPullerInfluenceSouth(int distance, Color tileColor, bool shouldHighlight)
    {
        if (!southWall && southNeighbor && distance > 0)
        {
            GridElement south = southNeighbor.GetComponent<GridElement>();
            south.GetComponent<Image>().color = tileColor;
            if (south.piece && south.piece.GetComponent<GamePiece>() is Unit) south.isHighlighted = shouldHighlight;
            else south.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            south.DisplayPullerInfluenceSouth(distance - 1, tileColor, shouldHighlight);
        }
    }

    private void DisplayPullerInfluenceWest(int distance, Color tileColor, bool shouldHighlight)
    {
        if (!westWall && westNeighbor && distance > 0)
        {
            GridElement west = westNeighbor.GetComponent<GridElement>();
            west.GetComponent<Image>().color = tileColor;
            if (west.piece && west.piece.GetComponent<GamePiece>() is Unit) west.isHighlighted = shouldHighlight;
            else west.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            west.DisplayPullerInfluenceWest(distance - 1, tileColor, shouldHighlight);
        }
    }
    #endregion
}
