using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMenu  : Menu
{
    [Header("Colors for UI Grid Elements")]
    public Color activeColor;    // the color for active UI elements
    public Color moveColor;      // the color of elements that can be moved to

    [Header("GameObject the Player has pressed a button on")]
    public GameObject activeGO;

    public GameObject selectedPiece;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (activeUIMenu)
        {
            if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") > 0)
                SelectElement(selectedGO.GetComponent<GridElement>().eastNeighbor);
            if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") < 0)
                SelectElement(selectedGO.GetComponent<GridElement>().westNeighbor);
            if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") > 0)
                SelectElement(selectedGO.GetComponent<GridElement>().northNeighbor);
            if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") < 0)
                SelectElement(selectedGO.GetComponent<GridElement>().southNeighbor);

            prevHorAxis = Input.GetAxisRaw("Horizontal");
            prevVerAxis = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                ActivateElement();

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
                Cancel();
        }
    }

    void ActivateElement()
    {
        // ASSUMING NOTHING HAS BEEN SELECTED...
        // Check to see if there's a piece above the selected tile
        //      If there is, activate this tile and open that piece's context menu
        //      If that piece wants to move, it should call what this used to do but is now in a public function
        //          This should use the Unit script's numMoves as a parameter for showing movement
        //      If there is not, do nothing
        if (activeGO == null)
        {
            if (selectedGO.GetComponent<GridElement>().piece != null)
            {
                selectedPiece = selectedGO.GetComponent<GridElement>().piece;
                selectedPiece.GetComponent<Unit>().ShowContextMenu();
            }
        }

        // ASSUMING SOMETHING *HAS* BEEN SELECTED (that is, we're already displaying a movement grid
        // Check to see if this element is a valid movement target for the piece
        //      If it is, turn off the movement grid and move the piece here
        //      If it isn't, cancel the move action? Do nothing? Do nothing for now
        else
        {
            // don't do anything if we reselect the active game object
            if (selectedGO == activeGO)
                return;
            else if (selectedPiece != null)
                if (selectedGO.GetComponent<GridElement>().isHighlighted)
                {
                    selectedPiece.GetComponent<Unit>().PerformAction(selectedGO);
                }
        }
    }

    void Cancel()
    {
        if (!selectedPiece) return;
        selectedPiece.GetComponent<Unit>().HideMovementGrid();
    }

    protected override void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;

        selectedGO.GetComponent<Image>().color = prevColor;
        selectedGO = newElement;
        prevColor = selectedGO.GetComponent<Image>().color;
        if (activeGO != null)
        {
            if (selectedGO == activeGO)
                selectedGO.GetComponent<Image>().color = activeColor;
            else
                selectedGO.GetComponent<Image>().color = selectedColor;
        }
        else
            selectedGO.GetComponent<Image>().color = selectedColor;
    }

    public void SetElementColor(GameObject element, Color newColor)
    {
        prevColor = element.GetComponent<Image>().color;
        element.GetComponent<Image>().color = newColor;
    }

    public void SetElementColor(GameObject element, Color newColor, Color newPrevColor)
    {
        prevColor = newPrevColor;
        element.GetComponent<Image>().color = newColor;
    }
}
