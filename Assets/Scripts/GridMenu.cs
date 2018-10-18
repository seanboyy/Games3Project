using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMenu  : Menu
{
    [Header("Colors for UI Grid Elements")]
    [SerializeField]
    public static Color activeColor = Color.blue;    // the color for active UI elements
    [SerializeField]
    public static Color moveColor = Color.yellow;      // the color of elements that can be moved to
    [SerializeField]
    public static Color pushColor = new Color(0.4F, 0, 1);      // the color of elements that can be influenced by pusher
    [SerializeField]
    public static Color pullColor = new Color(1F, 0.4F, 0);      // the color of elements that can be influenced by puller

    [Header("GameObject the Player has pressed a button on")]
    public GameObject activeGO;

    [Header("GameManager")]
    public GameMan gameMan;
    public GameObject selectedPiece;

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

            if (canPressButtons)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                    ActivateElement();

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
                    Cancel();
            }
            else
                canPressButtons = true;
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
            activeGO = selectedGO;
            if (selectedGO.GetComponent<GridElement>().piece != null)
            {
                selectedPiece = selectedGO.GetComponent<GridElement>().piece;
                if(selectedPiece.GetComponent<GamePiece>() is Unit) selectedPiece.GetComponent<Unit>().ShowContextMenu();
                canPressButtons = false;
            }
            else if (selectedGO.GetComponent<GridElement>().spawnable)
            {
                // Display a ContextMenu with all the pieces that can be spawned
                contextMenu.ShowContextMenu(gameObject);
                // Move the canvas to SelectedGO's location
                contextMenu.menuCanvas.transform.position = selectedGO.transform.position;
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

    void Cancel()
    {
        if (!selectedPiece) return;
        if(selectedPiece.GetComponent<GamePiece>() is Unit) selectedPiece.GetComponent<Unit>().HideMovementGrid();
        selectedPiece = null;
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

    public void PlaceUnit(UnitType unitType)
    {
        switch(unitType)
        {
            case UnitType.Unit:
                gameMan.PlaceUnit(selectedGO);
                break;
        };
        contextMenu.HideContextMenu();
        SetElementColor(selectedGO, selectedColor, defaultColor);
        activeGO = null;
    }

    public void PlaceUnit(string unitType)
    {
        if (unitType == "unit")
            PlaceUnit(UnitType.Unit);
    }
}
