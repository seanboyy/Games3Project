using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public const int MAX_NUM_MOVES = 2;
    public int remainingMoves = 2;

    public GameObject grid;

    private PopUp_Menu contextMenu;

	// Use this for initialization
	void Start ()
    {
        contextMenu = GetComponent<PopUp_Menu>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ActivateMoveButton()
    {
        grid.GetComponent<UI_InputMan>().ToggleMovement(remainingMoves);
        // close the pop up menu and reactivate the grid
        // ask the Unit script how many moves this piece gets
        // use the grid to display the movement possibilities
    }
}
