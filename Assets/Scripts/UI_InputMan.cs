using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputMan : MonoBehaviour
{
    [Header("Colors for UI Grid Elements")]
    public Color defaultColor;   // the color for non-active, non-selected UI elements
    public Color selectedColor;  // the color for non-active, selected UI elements
    public Color activeColor;    // the color for active UI elements

    [Header("GameObject the Player is currently on")]
    public GameObject selectedGO;
    [Header("GameObject the Player has pressed a button on")]
    public GameObject activeGO;


	// Use this for initialization
	void Start ()
    {
        SelectElement(selectedGO);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.JoystickButton3))
            SelectElement(selectedGO.GetComponent<ElementButton>().northNeighbor);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.JoystickButton2))
            SelectElement(selectedGO.GetComponent<ElementButton>().eastNeighbor);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.JoystickButton0))
            SelectElement(selectedGO.GetComponent<ElementButton>().southNeighbor);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.JoystickButton1))
            SelectElement(selectedGO.GetComponent<ElementButton>().westNeighbor);

    }

    void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;
        selectedGO.GetComponent<Image>().color = defaultColor;
        selectedGO = newElement;
        selectedGO.GetComponent<Image>().color = selectedColor;

    }
}
