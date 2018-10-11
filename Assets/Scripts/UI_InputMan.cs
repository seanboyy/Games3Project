using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputMan : Menu
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
                SelectElement(selectedGO.GetComponent<ElementButton>().eastNeighbor);
            if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") < 0)
                SelectElement(selectedGO.GetComponent<ElementButton>().westNeighbor);
            if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") > 0)
                SelectElement(selectedGO.GetComponent<ElementButton>().northNeighbor);
            if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") < 0)
                SelectElement(selectedGO.GetComponent<ElementButton>().southNeighbor);

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
            // set up raycast to look for a piece above this tile
            Ray ray;
            RaycastHit info;

            // Look towards the camera (-z direction)
            ray = new Ray(selectedGO.transform.position, new Vector3(0, 0, -1));
            Physics.Raycast(ray, out info);
            if (info.collider != null)
            {
                // look for a pop up menu, indicating this is a movable piece
                if (info.collider.gameObject.transform.root.gameObject.GetComponent<PopUp_Menu>() != null)
                {
                    Debug.Log("UI_InputMan::ActivateElement() - Found a piece above " + selectedGO.name);
                    selectedPiece = info.collider.gameObject.transform.root.gameObject;
                    selectedPiece.GetComponent<PopUp_Menu>().ShowContextMenu(gameObject);
                }
                else
                    Debug.Log("UI_InputMan::ActivateElement() - ERROR - Found a non-unit piece: " + info.collider.gameObject.name);
            }
            else
            {
                Debug.Log("UI_InputMan::ActivateElement() - No pieces found above " + selectedGO.name);
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
                if (selectedGO.GetComponent<ElementButton>().canMoveHere)
                {
                    selectedPiece.transform.position = new Vector3 (selectedGO.transform.position.x - 0.5f, selectedGO.transform.parent.transform.position.y - 0.5f);
                    activeGO.GetComponent<ElementButton>().DisplayMoveTiles(selectedPiece.GetComponent<Unit>().remainingMoves, defaultColor, false);
                    activeGO = null;
                    selectedGO.GetComponent<Image>().color = selectedColor;
                    prevColor = defaultColor;
                }
        }
    }

    void Cancel()
    {
        activeGO.GetComponent<ElementButton>().DisplayMoveTiles(selectedPiece.GetComponent<Unit>().remainingMoves, defaultColor, false);
        activeGO = null;
        selectedGO.GetComponent<Image>().color = selectedColor;
        prevColor = defaultColor;
    }

    public void ToggleMovement(int moves)
    {
        // Hide the movement grid
        if (activeGO != null)
        {
            activeGO.GetComponent<ElementButton>().DisplayMoveTiles(moves, defaultColor, false);
            activeGO.GetComponent<Image>().color = defaultColor;
        }
        // Hide the movement grid
        if (activeGO == selectedGO)
        {
            activeGO.GetComponent<ElementButton>().DisplayMoveTiles(moves, defaultColor, false);
            activeGO.GetComponent<Image>().color = selectedColor;
            activeGO = null;
            return;
        }

        // Show the movement grid
        activeGO = selectedGO;
        activeGO.GetComponent<ElementButton>().DisplayMoveTiles(moves, moveColor, true);
        activeGO.GetComponent<Image>().color = activeColor;
        prevColor = moveColor;
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
}
