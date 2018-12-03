using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedGridElement : NetworkBehaviour
{
    [Header("Can a piece spawn here?")]
    public bool spawnable = true; // Can a unit be spawned here?
    public PlayerEnum owner = PlayerEnum.none;
    public bool portal = false;
    public PlayerEnum portalOwner = PlayerEnum.none;
    [Header("Is this a goal element?")]
    public bool goal = false;

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
        if (spawnable || portal)
            GetComponent<Image>().color = GridMenu.spawnColor;
        if (goal)
            GetComponent<Image>().color = GridMenu.goalColor;
    }

    public void ChangeColor(Color color)
    {
        Image image = GetComponent<Image>();
        if (color == Menu.defaultColor)
        {
            if (spawnable || portal)
                color = GridMenu.spawnColor;
            if (goal)
                color = GridMenu.goalColor;
        }
        image.color = color;
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
        ray = new Ray(transform.position, Vector3.right);
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            eastNeighbor = info.collider.gameObject;
        }

        // look to the left (west)
        ray = new Ray(transform.position, Vector3.left);
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            westNeighbor = info.collider.gameObject;
        }

        // look to the up (north)
        ray = new Ray(transform.position, Vector3.up);
        Physics.Raycast(ray, out info);
        if (info.collider != null)
        {
            northNeighbor = info.collider.gameObject;
        }

        // look to the down (south)
        ray = new Ray(transform.position, Vector3.down);
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
    public void DisplayMoveTiles(int movesRemaining, bool showingMoves)
    {
        isHighlighted = showingMoves;
        if (showingMoves)
        {
            distance = movesRemaining;
            ChangeColor(GridMenu.moveColor);
        }
        else
        {
            distance = -1;
            ChangeColor(GridMenu.defaultColor);
        }
        if (movesRemaining <= 0)
            return;
        if (!eastWall && eastNeighbor != null)
            eastNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, showingMoves);
        if (!westWall && westNeighbor != null)
            westNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, showingMoves);
        if (!northWall && northNeighbor != null)
            northNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, showingMoves);
        if (!southWall && southNeighbor != null)
            southNeighbor.GetComponent<GridElement>().DisplayMoveTiles(movesRemaining - 1, showingMoves);
    }

    #region Pusher Influence
    public void DisplayPusherInfluence(bool shouldHighlight)
    {
        if (shouldHighlight)
            ChangeColor(GridMenu.pushColor);
        else
            ChangeColor(Menu.defaultColor);
        DisplayPusherInfluenceNorth(shouldHighlight, true);
        DisplayPusherInfluenceEast(shouldHighlight, true);
        DisplayPusherInfluenceSouth(shouldHighlight, true);
        DisplayPusherInfluenceWest(shouldHighlight, true);
    }

    private void DisplayPusherInfluenceNorth(bool shouldHighlight, bool immediateNeighbor)
    {
        if (!northWall && northNeighbor && northNeighbor.GetComponent<GridElement>().piece &&
            (northNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit ||
             northNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Trap))
        {
            GridElement north = northNeighbor.GetComponent<GridElement>();
            north.isHighlighted = shouldHighlight;
            if (shouldHighlight)
            {
                north.ChangeColor(GridMenu.pushColor);
                if (!immediateNeighbor)
                    north.ChangeColor(GridMenu.pushColor + new Color(0.4F, 0.4F, 0.4F));
            }
            else
                north.ChangeColor(Menu.defaultColor);
            north.DisplayPusherInfluenceNorth(shouldHighlight, false);
        }
    }

    private void DisplayPusherInfluenceEast(bool shouldHighlight, bool immediateNeighbor)
    {
        if (!eastWall && eastNeighbor && eastNeighbor.GetComponent<GridElement>().piece &&
            (eastNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit ||
             eastNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Trap))
        {
            GridElement east = eastNeighbor.GetComponent<GridElement>();
            east.isHighlighted = shouldHighlight;
            if (shouldHighlight)
            {
                east.ChangeColor(GridMenu.pushColor);
                if (!immediateNeighbor)
                    east.ChangeColor(GridMenu.pushColor + new Color(0.4F, 0.4F, 0.4F));
            }
            else
                east.ChangeColor(Menu.defaultColor);
            east.DisplayPusherInfluenceEast(shouldHighlight, false);
        }
    }

    private void DisplayPusherInfluenceSouth(bool shouldHighlight, bool immediateNeighbor)
    {
        if (!southWall && southNeighbor && southNeighbor.GetComponent<GridElement>().piece &&
            (southNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit ||
             southNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Trap))
        {
            GridElement south = southNeighbor.GetComponent<GridElement>();
            south.isHighlighted = shouldHighlight;
            if (shouldHighlight)
            {
                south.ChangeColor(GridMenu.pushColor);
                if (!immediateNeighbor)
                    south.ChangeColor(GridMenu.pushColor + new Color(0.4F, 0.4F, 0.4F));
            }
            else
                south.ChangeColor(Menu.defaultColor);
            south.DisplayPusherInfluenceSouth(shouldHighlight, false);
        }
    }

    private void DisplayPusherInfluenceWest(bool shouldHighlight, bool immediateNeighbor)
    {
        if (!westWall && westNeighbor && westNeighbor.GetComponent<GridElement>().piece &&
            (westNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Unit ||
             westNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>() is Trap))
        {
            GridElement west = westNeighbor.GetComponent<GridElement>();
            west.isHighlighted = shouldHighlight;
            if (shouldHighlight)
            {
                west.ChangeColor(GridMenu.pushColor);
                if (!immediateNeighbor)
                    west.ChangeColor(GridMenu.pushColor + new Color(0.4F, 0.4F, 0.4F));
            }
            else
                west.ChangeColor(Menu.defaultColor);
            west.DisplayPusherInfluenceWest(shouldHighlight, false);
        }
    }
    #endregion

    #region Puller Influence
    public void DisplayPullerInfluence(int distance, bool shouldHighlight)
    {
        DisplayPullerInfluenceNorth(distance, shouldHighlight);
        DisplayPullerInfluenceEast(distance, shouldHighlight);
        DisplayPullerInfluenceSouth(distance, shouldHighlight);
        DisplayPullerInfluenceWest(distance, shouldHighlight);
    }

    private void DisplayPullerInfluenceNorth(int distance, bool shouldHighlight)
    {
        if (!northWall && northNeighbor && distance > 0)
        {
            GridElement north = northNeighbor.GetComponent<GridElement>();
            if (shouldHighlight)
            {
                north.ChangeColor(GridMenu.pullColor);
                if (north.piece && (north.piece.GetComponent<GamePiece>() is Unit || north.piece.GetComponent<GamePiece>() is Trap)) north.isHighlighted = shouldHighlight;
                else north.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            }
            else
                north.ChangeColor(Menu.defaultColor);
            north.DisplayPullerInfluenceNorth(distance - 1, shouldHighlight);
        }
    }

    private void DisplayPullerInfluenceEast(int distance, bool shouldHighlight)
    {
        if (!eastWall && eastNeighbor && distance > 0)
        {
            GridElement east = eastNeighbor.GetComponent<GridElement>();
            if (shouldHighlight)
            {
                east.ChangeColor(GridMenu.pullColor);
                if (east.piece && (east.piece.GetComponent<GamePiece>() is Unit || east.piece.GetComponent<GamePiece>() is Trap)) east.isHighlighted = shouldHighlight;
                else east.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            }
            else
                east.ChangeColor(Menu.defaultColor);
            east.DisplayPullerInfluenceEast(distance - 1, shouldHighlight);
        }
    }

    private void DisplayPullerInfluenceSouth(int distance, bool shouldHighlight)
    {
        if (!southWall && southNeighbor && distance > 0)
        {
            GridElement south = southNeighbor.GetComponent<GridElement>();
            if (shouldHighlight)
            {
                south.ChangeColor(GridMenu.pullColor);
                if (south.piece && (south.piece.GetComponent<GamePiece>() is Unit || south.piece.GetComponent<GamePiece>() is Trap)) south.isHighlighted = shouldHighlight;
                else south.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            }
            else
                south.ChangeColor(Menu.defaultColor);
            south.DisplayPullerInfluenceSouth(distance - 1, shouldHighlight);
        }
    }

    private void DisplayPullerInfluenceWest(int distance, bool shouldHighlight)
    {
        if (!westWall && westNeighbor && distance > 0)
        {
            GridElement west = westNeighbor.GetComponent<GridElement>();
            if (shouldHighlight)
            {
                west.ChangeColor(GridMenu.pullColor);
                if (west.piece && (west.piece.GetComponent<GamePiece>() is Unit || west.piece.GetComponent<GamePiece>() is Trap)) west.isHighlighted = shouldHighlight;
                else west.GetComponent<Image>().color += new Color(0.2F, 0.2F, 0.2F);
            }
            else
                west.ChangeColor(Menu.defaultColor);
            west.DisplayPullerInfluenceWest(distance - 1, shouldHighlight);
        }
    }
    #endregion
}
