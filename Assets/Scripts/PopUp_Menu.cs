using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp_Menu : Menu
{
    public GameObject menuCanvas;

    private GameObject prevMenu;
    private bool canPressButtons = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (activeUIMenu)
        {
            // Move left or move up
            if ((prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") < 0) || (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") > 0))
            {
                SelectElement(selectedGO.GetComponent<PopUp_Button>().northNeighbor);
            }
            // Move right or move down
            if ((prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") > 0) || (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") < 0))
            {
                SelectElement(selectedGO.GetComponent<PopUp_Button>().southNeighbor);
            }

            prevHorAxis = Input.GetAxisRaw("Horizontal");
            prevVerAxis = Input.GetAxisRaw("Vertical");

            if (canPressButtons)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    selectedGO.GetComponent<Button>().onClick.Invoke();
                }
            }
            else
                canPressButtons = true;
        }
    }

    public void ShowContextMenu(GameObject callingMenu)
    {
        prevMenu = callingMenu;
        prevMenu.GetComponent<Menu>().activeUIMenu = false;
        activeUIMenu = true;
        menuCanvas.SetActive(true);
    }

    public void HideContextMenu()
    {
        Debug.Log("PopUp_Menu::HideContextMenu() - Hiding Menu");
        activeUIMenu = false;
        menuCanvas.SetActive(false);
        prevMenu.GetComponent<Menu>().activeUIMenu = true;
        prevMenu = null;
        canPressButtons = false;
    }

}
