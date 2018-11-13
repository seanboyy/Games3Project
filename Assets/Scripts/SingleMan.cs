using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleMan : GameMan
{
    public bool limitedMoves = false;
    public int moveLimit = 5;

    public Text turnsUsedText;
    public Text instructions1;
    public Text instructions2;

    private int turnsUsed = 0;
    private bool nextLevel = false;
    private bool gameOver = false; //Player has lost

    void Start()
    {
        if (limitedMoves)
            turnsUsedText.text = "Turns Remaining: " + (moveLimit - turnsUsed);
    }

    private void Update()
    {
        if (!nextLevel && !gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2)) EndTurn();
        }
        else if (nextLevel)
        {
            if (Input.anyKeyDown) SceneManager.LoadScene(nextSceneName);
        }
        else if (gameOver)
        {
            if (Input.anyKeyDown) SceneManager.LoadScene("menu");
        }

    }

    public override void EndLevel()
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

    public override void EndGame()
    {
        gameOver = true;
        instructions1.gameObject.SetActive(true);
        instructions2.gameObject.SetActive(true);
        instructions1.text = "Sorry! You lose!\n\nPress any key to continue";
        instructions2.text = "Sorry! You lose!\n\nPress any key to continue";
    }

    public override void EndTurn()
    {
        base.EndTurn();
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

    public override void HandleHorizontalMovement(GameObject player, float horizontal)
    {
        activeMenu.HandleHorizontalMovement(horizontal);
    }

    public override void HandleVerticalMovement(GameObject player, float vertical)
    {
        activeMenu.HandleVerticalMovement(vertical);
    }

    public override void HandleCrossButton(GameObject player)
    {
        activeMenu.HandleCrossButton();
    }

    public override void HandleTriangleButton(GameObject player)
    {
        activeMenu.HandleTriangleButton();
    }

    public override void HandleCircleButton(GameObject player)
    {
        activeMenu.HandleCircleButton();
    }

    public override void HandleSquareButton(GameObject player)
    {
        activeMenu.HandleSquareButton();
    }
}
