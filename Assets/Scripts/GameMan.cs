using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class GameMan : MonoBehaviour
{
    public string nextSceneName;
    public GameObject activePlayer;

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

    // Handle up/down left/right inputs
    public abstract void HandleMovement(Player player, float horizontal, float vertical);

    // Handle the X button being pressed
    public abstract void HandleCrossButton(Player player);

    // Handle the triangle button being pressed
    public abstract void HandleTriangleButton(Player player);

    // Handle the circle button being pressed
    public abstract void HandleCircleButton(Player player);

    // Handle the square button being pressed
    public abstract void HandleSquareButton(Player player);
}
