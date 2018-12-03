using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : Menu
{
    public GameObject menuCanvas;

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
        if (activeUIMenu && !canPressButtons)
            canPressButtons = true;
    }

    public void ShowContextMenu(Menu callingMenu)
    {
        prevMenu = callingMenu;
        prevMenu.activeUIMenu = false;
        if (prevMenu is GridMenu)
        {
            IGameMan gameMan = prevMenu.GetComponent<GridMenu>().gameMan;
            if (gameMan is MultiMan)
            {
                if (((MultiMan)gameMan).player1) ((MultiMan)gameMan).player1.GetComponent<Player>().SetActiveMenu(this);
                if (((MultiMan)gameMan).player2) ((MultiMan)gameMan).player2.GetComponent<Player>().SetActiveMenu(this);
            }
            if (gameMan is SingleMan)
            {
                if (((SingleMan)gameMan).activePlayer) ((SingleMan)gameMan).activePlayer.GetComponent<Player>().SetActiveMenu(this);
            }
        }
        activeUIMenu = true;
        menuCanvas.SetActive(true);
    }

    public void HideContextMenu()
    {
        activeUIMenu = false;
        menuCanvas.SetActive(false);
        prevMenu.GetComponent<Menu>().activeUIMenu = true;
        if (prevMenu is GridMenu)
        {
            IGameMan gameMan = prevMenu.GetComponent<GridMenu>().gameMan;
            if (gameMan is MultiMan)
            {
                if (((MultiMan)gameMan).player1) ((MultiMan)gameMan).player1.GetComponent<Player>().SetActiveMenu((GridMenu)prevMenu);
                if (((MultiMan)gameMan).player2) ((MultiMan)gameMan).player2.GetComponent<Player>().SetActiveMenu((GridMenu)prevMenu);
            }
            if (gameMan is SingleMan)
            {
                if (((SingleMan)gameMan).activePlayer) ((SingleMan)gameMan).activePlayer.GetComponent<Player>().SetActiveMenu((GridMenu)prevMenu);
            }
        }
        prevMenu = null;
        canPressButtons = false;
    }

    public void Cancel()
    {
        if (prevMenu.GetComponent<Menu>() is GridMenu)
        {
            GridMenu gm = prevMenu.GetComponent<GridMenu>();
            gm.activeGO = null;
            gm.SetElementColor(gm.selectedGO, Menu.selectedColor, Menu.defaultColor);
            HideContextMenu();
        }
    }

    public override void HandleHorizontalMovement(float horizontal)
    {
        if (!activeUIMenu) return;
        if (horizontal > 0)
            SelectElement(selectedGO.GetComponent<ContextButton>().southNeighbor);
        else
            SelectElement(selectedGO.GetComponent<ContextButton>().northNeighbor);
    }

    public override void HandleVerticalMovement(float vertical)
    {
        if (!activeUIMenu) return;
        if (vertical > 0)
            SelectElement(selectedGO.GetComponent<ContextButton>().northNeighbor);
        else
            SelectElement(selectedGO.GetComponent<ContextButton>().southNeighbor);
    }

    public override void HandleCrossButton()
    {
        if (!activeUIMenu || !canPressButtons) return;
        selectedGO.GetComponent<Button>().onClick.Invoke();
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
        throw new System.NotImplementedException();
    }

}
