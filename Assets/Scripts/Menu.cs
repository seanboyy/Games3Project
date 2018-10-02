using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [Header("Colors for UI Elements")]
    public Color defaultColor;   // the color for non-selected Buttons
    public Color selectedColor;  // the color for non-active, selected UI elements

    public GameObject selectedGO;

    [Header("Is this menu under active player control?")]
    public bool activeUIMenu = false;

    protected float prevHorAxis = 0;
    protected float prevVerAxis = 0;

    protected Color prevColor;    // the color of the previous selectedElement

    // Use this for initialization
    protected virtual void Start () {
        SelectElement(selectedGO);
        prevColor = defaultColor;
    }

    protected void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;
        selectedGO.GetComponent<Image>().color = prevColor;
        selectedGO = newElement;
        prevColor = selectedGO.GetComponent<Image>().color;
        selectedGO.GetComponent<Image>().color = selectedColor;
    }
}
