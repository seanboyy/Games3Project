using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedContextMenu : NetworkedMenu
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
        if (!activeUIMenu && isActiveAndEnabled)
            menuCanvas.SetActive(false);

    }

    public void ShowContextMenu(NetworkedMenu callingMenu)
    {
        prevMenu = callingMenu;
        prevMenu.activeUIMenu = false;
        activeUIMenu = true;
        if (prevMenu is NetworkedGridMenu)
        {
            if (((NetworkedGridMenu)prevMenu).activePlayer)
            {
                ((NetworkedGridMenu)prevMenu).activePlayer.SetActiveMenu(gameObject);
            }
            //MultiMan gameMan = prevMenu.GetComponent<NetworkedGridMenu>().gameMan;
            //if (gameMan.activePlayer) gameMan.activePlayer.GetComponent<NetworkedPlayer>().SetActiveMenu(gameObject);
        }
        menuCanvas.SetActive(true);
    }

    public void HideContextMenu()
    {
        if (prevMenu == null) return;
        activeUIMenu = false;
        menuCanvas.SetActive(false);
        prevMenu.activeUIMenu = true;
        if (prevMenu is NetworkedGridMenu)
        {
            MultiMan gameMan = prevMenu.GetComponent<NetworkedGridMenu>().gameMan;
            if (gameMan.activePlayer) gameMan.activePlayer.GetComponent<NetworkedPlayer>().CmdSetActiveMenu(prevMenu.gameObject);
        }
        prevMenu = null;
        canPressButtons = false;
    }

    public void Cancel()
    {
        if (prevMenu.GetComponent<NetworkedMenu>() is NetworkedGridMenu)
        {
            NetworkedGridMenu gm = prevMenu.GetComponent<NetworkedGridMenu>();
            gm.activeGO = null;
            gm.SetElementColor(gm.selectedGO, selectedColor, defaultColor);
            HideContextMenu();
        }
    }

    public override void HandleHorizontalMovement(float horizontal)
    {
        if (!activeUIMenu) return;
        if (horizontal > 0)
            SelectElement(selectedGO.GetComponent<NetworkedContextButton>().southNeighbor);
        else
            SelectElement(selectedGO.GetComponent<NetworkedContextButton>().northNeighbor);
    }

    public override void HandleVerticalMovement(float vertical)
    {
        if (!activeUIMenu) return;
        if (vertical > 0)
            SelectElement(selectedGO.GetComponent<NetworkedContextButton>().northNeighbor);
        else
            SelectElement(selectedGO.GetComponent<NetworkedContextButton>().southNeighbor);
    }

    public override void HandleCrossButton()
    {
        if (!activeUIMenu || !canPressButtons) return;
        selectedGO.GetComponent<Button>().onClick.Invoke();
    }

    public override void HandleTriangleButton()
    {
        //throw new System.NotImplementedException();
    }

    public override void HandleCircleButton()
    {
        if (!activeUIMenu || !canPressButtons) return;
        Cancel();
    }

    public override void HandleSquareButton()
    {
        //throw new System.NotImplementedException();
    }

    public override void HandleLeftShoulderBumper()
    {
        //throw new System.NotImplementedException();
    }

    public override void HandleRightShoulderBumper()
    {
        //throw new System.NotImplementedException();
    }
}
