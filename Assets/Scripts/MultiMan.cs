using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiMan : GameMan
{

    public GameObject player1;
    public GameObject player2;
    public Queue<GameObject> turnQueue;
    public Image turnArrow;
    private int turnCount = 0;

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
                turnArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
                turnCount = 1;
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
        if (Input.GetKeyDown(KeyCode.Return)) EndTurn();
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
        turnCount = ++turnCount % 2;
        StartCoroutine("FlipArrow");
        base.EndTurn();
    }

    private IEnumerator FlipArrow()
    {
        for(int i = 0; i < 20; ++i)
        {
            turnArrow.transform.rotation = Quaternion.Lerp(turnArrow.transform.rotation, Quaternion.Euler(0, 0, 180 * turnCount), Time.deltaTime * 20);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
