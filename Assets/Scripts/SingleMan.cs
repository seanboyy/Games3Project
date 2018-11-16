using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleMan : MonoBehaviour, IGameMan
{
    public string nextSceneName;
    public GameObject activePlayer;

    public Menu activeMenu;
    public Menu prevMenu;

    public bool limitedMoves = false;
    public int moveLimit = 5;

    public Text turnsUsedText;
    public Text instructions1;
    public Text instructions2;

    private int turnsUsed = 0;
    private bool nextLevel = false;
    private bool gameOver = false; //Player has lost
    private bool canPressButtons = false;

    void Start()
    {
        if (limitedMoves)
            turnsUsedText.text = "Turns Remaining: " + (moveLimit - turnsUsed);
        if (activePlayer && !activePlayer.activeInHierarchy)
            activePlayer.SetActive(true);
        if (activeMenu is GridMenu) ((GridMenu)activeMenu).TurnOnChildren();
    }

    private void Update()
    {
        if (canPressButtons)
        {
            if (nextLevel)
            {
                if (Input.anyKeyDown) SceneManager.LoadScene(nextSceneName);
            }
            else if (gameOver)
            {
                if (Input.anyKeyDown) SceneManager.LoadScene("menu");
            }
        }
        else if (nextLevel || gameOver)
            canPressButtons = true;

    }

    public void EndLevel()
    {
        if (!limitedMoves)
            turnsUsedText.text = "Turns Used: " + ++turnsUsed;
        else
            turnsUsedText.text = "Turns Remaining: " + (moveLimit - ++turnsUsed);
        // Go to a menu between levels asking if you want to go to the next level, or if you want to return to the main menu
        // Right now, that's not implemented - you just go to wherever it's set in the editor
        nextLevel = true;
        instructions1.gameObject.SetActive(true);
        instructions2.gameObject.SetActive(true);
        instructions1.text = "Congratulations! You win!\n\nPress any key to continue";
        instructions2.text = "Congratulations! You win!\n\nPress any key to continue";
    }

    public void EndGame()
    {
        gameOver = true;
        instructions1.gameObject.SetActive(true);
        instructions2.gameObject.SetActive(true);
        instructions1.text = "Sorry! You lose!\n\nPress any key to continue";
        instructions2.text = "Sorry! You lose!\n\nPress any key to continue";
    }

    public void EndTurn()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.ResetPiece();
        }
        if (!limitedMoves)
            turnsUsedText.text = "Turns Used: " + ++turnsUsed;
        else
            turnsUsedText.text = "Turns Remaining: " + (moveLimit - ++turnsUsed);
        if (moveLimit <= turnsUsed)
        {
            // Game is over; player has lost
            EndGame();
        }

    }

    public void HandleHorizontalMovement(GameObject player, float horizontal)
    {
        Debug.Log("Handling Horizontal Player Input");
        activeMenu.HandleHorizontalMovement(horizontal);
    }

    public void HandleVerticalMovement(GameObject player, float vertical)
    {
        Debug.Log("Handling Vertical Player Input");
        activeMenu.HandleVerticalMovement(vertical);
    }

    public void HandleCrossButton(GameObject player)
    {
        activeMenu.HandleCrossButton();
    }

    public void HandleTriangleButton(GameObject player)
    {
        if (!nextLevel && !gameOver)
            EndTurn();
    }

    public void HandleCircleButton(GameObject player)
    {
        activeMenu.HandleCircleButton();
    }

    public void HandleSquareButton(GameObject player)
    {
        activeMenu.HandleSquareButton();
    }

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
}
