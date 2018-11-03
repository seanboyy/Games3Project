using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMan : MonoBehaviour
{
    public string nextSceneName;

    public bool limitedMoves = false;
    public int moveLimit = 5;

    public Text turnsUsedText;
    public Text instructions1;
    public Text instructions2;

    private int turnsUsed = 0;
    private bool nextLevel = false;
    private bool gameOver = false; //Player has lost

    public GameObject player1;  
    public GameObject player2;
    public PlayerEnum activePlayer = PlayerEnum.Player1;

	// Use this for initialization
	void Start ()
    {
        if (limitedMoves)
            turnsUsedText.text = "Turns Remaining: " + (moveLimit - turnsUsed);
    }

    void Update()
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

    public void PlaceUnit(GameObject location, UnitType type)
    {
        switch(activePlayer)
        {
            case PlayerEnum.Player1:
                if (player1) player1.GetComponent<Player>().PlaceUnit(location, type);
                else Debug.Log("GameMan::PlaceUnit - Player 1 not defined");
                break;
            case PlayerEnum.Player2:
                if (player2) player2.GetComponent<Player>().PlaceUnit(location, type);
                else Debug.Log("GameMan::PlaceUnit - Player 2 not defined");
                break;
            default:
                Debug.Log("GameMan::PlaceUnit - Player not recognized");
                break;
        };
    }


    public void ReturnUnit(GameObject unit, PlayerEnum owner)
    {
        switch (owner)
        {
            case PlayerEnum.Player1:
                if (player1) player1.GetComponent<Player>().ReturnUnit(unit);
                else Debug.Log("GameMan::PlaceUnit - Player 1 not defined");
                break;
            case PlayerEnum.Player2:
                if (player2) player2.GetComponent<Player>().ReturnUnit(unit);
                else Debug.Log("GameMan::PlaceUnit - Player 2 not defined");
                break;
            default:
                Debug.Log("GameMan::PlaceUnit - Player not recognized");
                break;
        };
    }

    // Goes to the next level
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

    // Goes to the menu
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
        if (player1 && player2)
        {
            if (activePlayer == PlayerEnum.Player1)
                activePlayer = PlayerEnum.Player2;
            else
                activePlayer = PlayerEnum.Player1;
        }
        foreach(Unit unit in FindObjectsOfType<Unit>())
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
}
