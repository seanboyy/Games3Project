using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMan : GameMan
{

    public GameObject player1;
    public GameObject player2;
    public Queue<GameObject> turnQueue;

    // Use this for initialization
    void Start ()
    {
        turnQueue = new Queue<GameObject>();
        if (player1 && player2)
        {
            if (Random.Range(0F, 1F) < 0.5)
            {
                activePlayer = player1;
                turnQueue.Enqueue(player2);
                turnQueue.Enqueue(player1);
            }
            else
            {
                activePlayer = player2;
                turnQueue.Enqueue(player1);
                turnQueue.Enqueue(player2);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void EndGame()
    {

    }

    public override void EndLevel()
    {
        throw new System.NotImplementedException();
    }

    public override void EndTurn()
    {
        if (player1 && player2)
        {
            turnQueue.Enqueue(activePlayer);
            activePlayer = turnQueue.Dequeue();
        }
        base.EndTurn();
    }
}
