using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour {

    [Header("Colors for UI Elements")]
    public static Color defaultColor = Color.white;   // the color for non-selected Buttons
    public static Color selectedColor = Color.cyan;  // the color for non-active, selected UI elements

    public GameObject selectedGO;

    [Header("Is this menu under active player control?")]
    public bool activeUIMenu = false;

    protected float prevHorAxis = 0;
    protected float prevVerAxis = 0;

    protected Color prevColor;    // the color of the previous selectedElement

    // Use this for initialization
    protected virtual void Start ()
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
    public abstract void HandleMovement(float horizontal, float vertical);

    // Handle the X button being pressed
    public abstract void HandleCrossButton();

    // Handle the triangle button being pressed
    public abstract void HandleTriangleButton();

    // Handle the circle button being pressed
    public abstract void HandleCircleButton();

    // Handle the square button being pressed
    public abstract void HandleSquareButton();

}
