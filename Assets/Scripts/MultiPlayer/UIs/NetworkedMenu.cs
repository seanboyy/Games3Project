using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public abstract class NetworkedMenu : NetworkBehaviour
{

    [Header("Colors for UI Elements")]
    public static Color defaultColor = Color.white;   // the color for non-selected Buttons
    public static Color selectedColor = Color.cyan;  // the color for non-active, selected UI elements

    public GameObject selectedGO;

    [Header("Is this menu under active player control?")]
    public bool activeUIMenu = false;

    [SerializeField]
    protected Color prevColor;    // the color of the previous selectedElement
    [SerializeField]
    protected NetworkedMenu prevMenu;


    // Use this for initialization
    protected virtual void Start()
    {
        SelectElement(selectedGO);
        prevColor = defaultColor;
    }

    protected virtual void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;
        selectedGO.GetComponent<Image>().color = prevColor;
        selectedGO = newElement;
        prevColor = selectedGO.GetComponent<Image>().color;
        selectedGO.GetComponent<Image>().color = selectedColor;
    }

    // Handle up/down left/right inputs
    public abstract void HandleHorizontalMovement(float horizontal);

    // Handle left/right inputs
    public abstract void HandleVerticalMovement(float vertical);

    // Handle the X button being pressed
    public abstract void HandleCrossButton();

    // Handle the triangle button being pressed
    public abstract void HandleTriangleButton();

    // Handle the circle button being pressed
    public abstract void HandleCircleButton();

    // Handle the square button being pressed
    public abstract void HandleSquareButton();
    
    public abstract void HandleLeftShoulderBumper();

    public abstract void HandleRightShoulderBumper();

}
