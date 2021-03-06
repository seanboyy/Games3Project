﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMenu  : Menu
{
    [Header("Colors for UI Grid Elements")]
    public static Color activeColor = Color.blue;                   // the color for active UI elements
    public static Color moveColor = Color.yellow;                   // the color of elements that can be moved to
    public static Color pushColor = new Color(0.4F, 0, 1);          // the color of elements that can be influenced by pusher
    public static Color pullColor = new Color(1F, 0.4F, 0);         // the color of elements that can be influenced by puller
    public static Color spawnColor = new Color(0.6F, 1, 0.6F);      // the color of elements that can hold spawn pieces
    public static Color goalColor = new Color(1, 0.6F, 1);          // the color of elements that are goal zones

    [Header("GameObject the Player has pressed a button on")]
    public GameObject activeGO;

    [Header("GameManager")]
    public SingleMan gameMan;
    public GameObject selectedPiece;
    public Player activePlayer;
    public Text pieceDescription;

    [Header("Other Miscellaneous")]
    public Sprite NoWalls;
    public Sprite OneWall;
    public Sprite TwoWallsL;
    public Sprite TwoWallsHall;
    public Sprite ThreeWalls;
    public Sprite FourWalls;

    private ContextMenu contextMenu;
    private bool canPressButtons = false;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        contextMenu = GetComponent<ContextMenu>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (activeUIMenu && !canPressButtons)
            canPressButtons = true;
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
            GridElement selectedGE = selectedGO.GetComponent<GridElement>();
            activeGO = selectedGO;
            if (selectedGE.piece && selectedGE.piece.GetComponent<GamePiece>() is Unit)
            {
                selectedPiece = selectedGE.piece;
                //if(selectedPiece.GetComponent<GamePiece>() is Unit) selectedPiece.GetComponent<Unit>().ShowContextMenu();
                //canPressButtons = false;
                if(selectedPiece.GetComponent<GamePiece>() is Unit) selectedPiece.GetComponent<Unit>().DisplayMoveGrid();
            }
            else if (!(selectedGE.piece && selectedGE.piece.GetComponent<GamePiece>() is Trap) &&  selectedGE.spawnable || selectedGE.portal) 
            {
                // Display a ContextMenu with all the pieces that can be spawned
                contextMenu.ShowContextMenu(this);
                // Move the canvas to SelectedGO's location
                contextMenu.menuCanvas.transform.position = selectedGO.transform.position + contextMenu.menuCanvas.transform.position.z * Vector3.forward;
                canPressButtons = false;
                // Long term, this must be dynamic, but we can settle for short term for now
            }
            else
            {
                activeGO = null;
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
                    SetElementColor(selectedGO, selectedColor, defaultColor);
                    activeGO = null;
                    selectedPiece = null;
                }
        }
    }

    public void ActivateElementAction()
    {

        if (activeGO == null)
        {
            GridElement selectedGE = selectedGO.GetComponent<GridElement>();
            activeGO = selectedGO;
            if (selectedGE.piece && selectedGE.piece.GetComponent<GamePiece>() is Unit)
            {
                selectedPiece = selectedGE.piece;
                if (selectedPiece.GetComponent<GamePiece>() is Unit)
                    if (!selectedPiece.GetComponent<Unit>().DisplayActionGrid())    // try to open the action grid, but if you can't (like with unit and portal placer)
                                                                                    // then deselect this GridElement
                        activeGO = null;
            }
            else
                activeGO = null;
        }
    }

    void Cancel()
    {
        if (!selectedPiece) return;
        if(selectedPiece.GetComponent<GamePiece>() is Unit) selectedPiece.GetComponent<Unit>().HideAction();
        selectedPiece = null;
    }

    public void ChangeElementSelected(GameObject newElement)
    {
        SelectElement(newElement);
    }

    protected override void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;

        selectedGO.GetComponent<GridElement>().ChangeColor(prevColor);
        selectedGO = newElement;
        prevColor = selectedGO.GetComponent<Image>().color;
        GridElement selectedGE = selectedGO.GetComponent<GridElement>();
        if (activeGO != null)
        {
            if (selectedGO == activeGO)
                selectedGE.ChangeColor(activeColor);
            else
                selectedGE.ChangeColor(selectedColor);
        }
        else
            selectedGO.GetComponent<GridElement>().ChangeColor(selectedColor);
        UpdateDescription();
    }

    public void UpdateDescription()
    {
        GridElement selectedGE = selectedGO.GetComponent<GridElement>();
        if (selectedGE && selectedGE.piece)
            pieceDescription.text = selectedGE.piece.name;
        else
        {
            pieceDescription.text = "";
        }
    }

    public void SetElementColor(GameObject element, Color newColor)
    {
        prevColor = element.GetComponent<Image>().color;
        element.GetComponent<GridElement>().ChangeColor(newColor);
    }

    public void SetElementColor(GameObject element, Color newColor, Color newPrevColor)
    {
        prevColor = newPrevColor;
        element.GetComponent<GridElement>().ChangeColor(newColor);
    }

    public void PlaceUnit(string unitType)
    {
        switch (unitType)
        {
            case "unit":
                activePlayer.PlaceUnit(selectedGO, UnitType.Unit);
                break;
            case "pusher":
                activePlayer.PlaceUnit(selectedGO, UnitType.Pusher);
                break;
            case "puller":
                activePlayer.PlaceUnit(selectedGO, UnitType.Puller);
                break;
            case "twister":
                activePlayer.PlaceUnit(selectedGO, UnitType.Twister);
                break;
            case "portalPlacer":
                activePlayer.PlaceUnit(selectedGO, UnitType.Portalist);
                break;
            default:
                Debug.Log("GridMenu::PlaceUnit() - Unit not recognized: " + unitType);
                break;
        };
        contextMenu.HideContextMenu();
        SetElementColor(selectedGO, selectedColor, defaultColor);
        activeGO = null;
    }

    public override void HandleHorizontalMovement(float horizontal)
    {
        if (!activeUIMenu) return;
        if (horizontal > 0)
            SelectElement(selectedGO.GetComponent<GridElement>().eastNeighbor);
        else
            SelectElement(selectedGO.GetComponent<GridElement>().westNeighbor);
    }

    public override void HandleVerticalMovement(float vertical)
    {
        if (!activeUIMenu) return;
        if (vertical > 0)
            SelectElement(selectedGO.GetComponent<GridElement>().northNeighbor);
        else
            SelectElement(selectedGO.GetComponent<GridElement>().southNeighbor);
    }

    public override void HandleCrossButton()
    {
        if (!activeUIMenu || !canPressButtons) return;
        ActivateElement();
    }

    public override void HandleTriangleButton()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleCircleButton()
    {
        if (!activeUIMenu || !canPressButtons) return;
        Cancel();
    }

    public override void HandleSquareButton()
    {
        ActivateElementAction();
    }

    public override void HandleLeftShoulderBumper()
    {
        activePlayer.RotateLeft(selectedGO.GetComponent<GridElement>());
    }

    public override void HandleRightShoulderBumper()
    {
        activePlayer.RotateRight(selectedGO.GetComponent<GridElement>());
    }
}
