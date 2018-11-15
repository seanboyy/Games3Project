using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MultiMan : GameMan
{

    public GameObject player1;
    public GameObject player2;
    public Queue<GameObject> turnQueue;
    public Image turnArrow;
    private int turnCount = 0;
    private bool justSwitched = false;

    // Use this for initialization
    void Start ()
    {
        SetupTurnQueue();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public override void EndGame()
    {
        EndLevel();
    }

    public override void EndLevel()
    {
        Debug.Log("Stopping Client");
        NetworkManager.singleton.StopClient();
        Debug.Log("Stopping Host");
        NetworkManager.singleton.StopHost();
        Debug.Log("Stopping Server");
        NetworkManager.singleton.StopServer();
        Debug.Log("Disconnecting connections");
        NetworkServer.DisconnectAll();
        SceneManager.LoadScene("lobby");
    }

    public override void EndTurn()
    {
        if (player1 && player2)
        {
            justSwitched = true;
            activePlayer = turnQueue.Dequeue();
            turnQueue.Enqueue(activePlayer);
            turnCount = ++turnCount % 2;
            StartCoroutine("FlipArrow");
        }
        base.EndTurn();
    }

    private IEnumerator FlipArrow()
    {
        if (!turnArrow)
        {
            Debug.Log("MultiMan::Flip Arrow() - Arrow not set");
            yield return null;
        }
        for(int i = 0; i < 20; ++i)
        {
            turnArrow.transform.rotation = Quaternion.Lerp(turnArrow.transform.rotation, Quaternion.Euler(0, 0, 180 * turnCount), Time.deltaTime * 20);
            yield return new WaitForEndOfFrame();
        }
        justSwitched = false;
        yield return null;
    }

    public override void HandleHorizontalMovement(GameObject player, float horizontal)
    {
        if (player == activePlayer)
            activeMenu.HandleHorizontalMovement(horizontal);
    }

    public override void HandleVerticalMovement(GameObject player, float vertical)
    {
        if (player == activePlayer)
            activeMenu.HandleVerticalMovement(vertical);
    }

    public override void HandleCrossButton(GameObject player)
    {
        if (player == activePlayer)
            activeMenu.HandleCrossButton();
    }

    public override void HandleTriangleButton(GameObject player)
    {
        if (player == activePlayer && !justSwitched)
            EndTurn();
    }

    public override void HandleCircleButton(GameObject player)
    {
        if (player == activePlayer)
            activeMenu.HandleCircleButton();
    }

    public override void HandleSquareButton(GameObject player)
    {
        if (player == activePlayer)
            activeMenu.HandleSquareButton();
    }

    public void RegisterPlayer(GameObject player)
    {
        if (!player1)
        {
            player1 = player;
            player.GetComponent<Player>().identity = PlayerEnum.Player1;
        }
        else if (!player2)
        {
            player2 = player;
            player.GetComponent<Player>().identity = PlayerEnum.Player2;
        }
        SetupTurnQueue();
    }

    private void SetupTurnQueue()
    {
        if(turnQueue == null) turnQueue = new Queue<GameObject>();
        if (player1 && player2)
        {
            if (Random.Range(0F, 1F) < 0.5)
            {
                activePlayer = player1;
                turnQueue.Enqueue(player2);
                turnQueue.Enqueue(player1);
                turnCount = 1;
            }
            else
            {
                activePlayer = player2;
                turnQueue.Enqueue(player1);
                turnQueue.Enqueue(player2);
            }
        }
        StartCoroutine("FlipArrow");
    }
}
