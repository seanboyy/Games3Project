using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class GameMan : MonoBehaviour
{
    public string nextSceneName;
    public GameObject activePlayer;

    public Menu activeMenu;
    public Menu prevMenu;

    public void SetActiveMenu(Menu newMenu)
    {
        prevMenu = activeMenu;
        activeMenu = newMenu;
    }

    public void PlaceUnit(GameObject location, UnitType type)
    {
        if (activePlayer) activePlayer.GetComponent<Player>().PlaceUnit(location, type);
        else Debug.Log("GameMan::PlaceUnit - Active Player not defined");
    }

    public void ReturnUnit(GameObject unit, GameObject owner)
    {
        if (owner.GetComponent<Player>())
            owner.GetComponent<Player>().ReturnUnit(unit);
    }

    // Goes to the next level
    public abstract void EndLevel();

    // Goes to the menu
    public abstract void EndGame();

    public virtual void EndTurn()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.ResetPiece();
        }
    }

    // Handle left/right inputs
    public abstract void HandleHorizontalMovement(GameObject player, float horizontal);
   
    // Handle left/right inputs
    public abstract void HandleVerticalMovement(GameObject player, float vertical);

    // Handle the X button being pressed
    public abstract void HandleCrossButton(GameObject player);

    // Handle the triangle button being pressed
    public abstract void HandleTriangleButton(GameObject player);

    // Handle the circle button being pressed
    public abstract void HandleCircleButton(GameObject player);

    // Handle the square button being pressed
    public abstract void HandleSquareButton(GameObject player);
}
