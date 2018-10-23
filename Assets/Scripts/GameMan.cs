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

    public GameObject unitPrefab;
    public GameObject pusherPrefab;
    public GameObject pullerPrefab;
    public GameObject twisterPrefab;
    public GameObject portalPlacerPrefab;

    private ObjectPool unitPool;
    private ObjectPool pusherPool;
    private ObjectPool pullerPool;
    private ObjectPool twisterPool;
    private ObjectPool portalPlacerPool;

    private int turnsUsed = 0;
    private bool nextLevel = false;
    private bool gameOver = false; //Player has lost
	// Use this for initialization
	void Start ()
    {
        unitPool = new ObjectPool(unitPrefab, false, 1);
        pusherPool = new ObjectPool(pusherPrefab, false, 1);
        pullerPool = new ObjectPool(pullerPrefab, false, 1);
        twisterPool = new ObjectPool(twisterPrefab, false, 1);
        portalPlacerPool = new ObjectPool(portalPlacerPrefab, false, 1);

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
        GameObject unitGO = null; 
        switch(type)
        {
            case UnitType.Unit:
                unitGO = unitPool.GetObject();
                break;
            case UnitType.Puller:
                unitGO = pullerPool.GetObject();
                break;
            case UnitType.Pusher:
                unitGO = pusherPool.GetObject();
                break;
            case UnitType.Twister:
                unitGO = twisterPool.GetObject();
                break;
            case UnitType.PortalPlacer:
                unitGO = portalPlacerPool.GetObject();
                break;
        }
        if (unitGO)
        {
            unitGO.GetComponent<Unit>().SetLocation(location);
            unitGO.GetComponent<Unit>().remainingMoves = 2;
        }
        else
            Debug.Log("GameMan::PlaceUnit() - Insufficient " + type + " units");
    }

    public void ReturnUnit(GameObject unit)
    {
        unit.transform.position = new Vector3(-50, -50, unit.transform.position.z);
        UnitType unitType = unit.GetComponent<Unit>().unitType;
        switch (unitType)
        {
            case UnitType.Unit:
                unitPool.ReturnObject(unit);
                break;
            case UnitType.Puller:
                pullerPool.ReturnObject(unit);
                break;
            case UnitType.Pusher:
                pusherPool.ReturnObject(unit);
                break;
            case UnitType.Twister:
                twisterPool.ReturnObject(unit);
                break;
            case UnitType.PortalPlacer:
                portalPlacerPool.ReturnObject(unit);
                break;
        }
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
