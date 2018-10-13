using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : GamePiece
{
    public const int MAX_NUM_MOVES = 2;
    public int remainingMoves = 2;

    public GridMenu grid;
    public GridElement gridElement; // what grid element this piece is on
    private ContextMenu contextMenu;

	// Use this for initialization
	void Start ()
    {
        contextMenu = GetComponent<ContextMenu>();
        FindGridElement();	
	}

    public void FindGridElement()
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
            if (foundGameObject.GetComponent<GridElement>() != null)
            {
                gridElement = foundGameObject.GetComponent<GridElement>();
                gridElement.piece = gameObject;
            }
            else
                Debug.Log("Unit::FindGridElement() - ERROR - Found a non-grid-element piece: " + foundGameObject.name);
        }
        else
        {
            Debug.Log("Unit::FindGridElement() - No elements found above " + name);
        }
    }

    public void ShowContextMenu()
    {
        contextMenu.ShowContextMenu(grid.gameObject);
    }

    public void HideContextMenu()
    {
        contextMenu.HideContextMenu();
    }

    public void ActivateMoveButton()
    {
        // Close the ContextMenu
        contextMenu.HideContextMenu();
        // Show the Movement Grid
        gridElement.DisplayMoveTiles(remainingMoves, grid.moveColor, true);
        grid.SetElementColor(gridElement.gameObject, grid.activeColor);
        grid.activeGO = gridElement.gameObject;
    }

    public void MoveUnit(GameObject newLoc)
    {
        // Get the distance to new element
        int distance = newLoc.GetComponent<GridElement>().distance;
        // Turn off the movement grid
        gridElement.DisplayMoveTiles(remainingMoves, grid.defaultColor, false);
        // Set the remaining moves appropriately
        remainingMoves = distance;
        // Go to the new element location
        transform.position = new Vector3(newLoc.transform.position.x - 0.5f, newLoc.transform.parent.transform.position.y - 0.5f, transform.position.z);
        grid.activeGO = null;
        gridElement.piece = null;
        // Update the gridElement this is on
        gridElement = newLoc.GetComponent<GridElement>();
        gridElement.piece = gameObject;
        grid.SetElementColor(gridElement.gameObject, grid.selectedColor, grid.defaultColor);
    }

    public void HideMovementGrid()
    {
        gridElement.DisplayMoveTiles(remainingMoves, grid.defaultColor, false);
        grid.activeGO = null;
        grid.SetElementColor(grid.selectedGO, grid.selectedColor, grid.defaultColor);
    }
}
