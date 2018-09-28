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

    private float prevHorAxis = 0;
    private float prevVerAxis = 0;

	// Use this for initialization
	void Start ()
    {
        SelectElement(selectedGO);
	}
	
	// Update is called once per frame
	void Update ()
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
            ActivateElement();
    }

    void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;
        if (selectedGO != activeGO)
            selectedGO.GetComponent<Image>().color = defaultColor;
        else
            selectedGO.GetComponent<Image>().color = activeColor;
        selectedGO = newElement;
        selectedGO.GetComponent<Image>().color = selectedColor;
    }

    void ActivateElement()
    {
        if (activeGO != null)
            activeGO.GetComponent<Image>().color = defaultColor;
        if (activeGO == selectedGO)
        {
            activeGO.GetComponent<Image>().color = selectedColor;
            activeGO = null;
            return;
        }
        activeGO = selectedGO;
        activeGO.GetComponent<Image>().color = activeColor;
    }
}
