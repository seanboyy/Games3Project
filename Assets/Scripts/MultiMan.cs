using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MultiMan : NetworkBehaviour, IGameMan
{

    public string nextSceneName;
    [SyncVar]
    public GameObject activePlayer;

    [SyncVar]
    public GameObject player1;
    [SyncVar]
    public GameObject player2;
    public Queue<GameObject> turnQueue;
    public Image turnArrow;
    [SyncVar]
    private int turnCount = 0;
    [SyncVar]
    private bool player1GoesFirst;
    private bool justSwitched = false;

    // Not sure if we want a reference to the grid, but...
    public GridMenu grid;

    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            player1GoesFirst = Random.Range(0F, 1F) < 0.5F;
            RpcDoTimeBar();
        }
        RegisterPlayers();       
    }

    void RegisterPlayers()
    {
        if (isServer)
        {
            if (FindObjectsOfType<Player>().Length < 2) return;
            player1 = FindObjectsOfType<Player>()[0].gameObject;
            player1.name = "Player1";
            player1.GetComponent<Player>().identity = PlayerEnum.Player1;
            player2 = FindObjectsOfType<Player>()[1].gameObject;
            player2.GetComponent<Player>().identity = PlayerEnum.Player2;
            player2.name = "Player2";
        }
        SetupTurnQueue();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player1 || !player2) RegisterPlayers();
    }

    public void EndGame()
    {
        EndLevel();
    }


    public void EndLevel()
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

    public void EndTurn()
    {
        if (player1 && player2)
        {
            justSwitched = true;
            activePlayer.GetComponent<Player>().activePlayer = false;
            activePlayer = turnQueue.Dequeue();
            activePlayer.GetComponent<Player>().activePlayer = true;
            turnQueue.Enqueue(activePlayer);
            turnCount = ++turnCount % 2;
            // Reason the arrow doesn't flip in sync is because this isn't an RPC
            StartCoroutine("FlipArrow");
        }
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.ResetPiece();
        }
        FindObjectOfType<TimeBar>().StopAllCoroutines();
        if (isServer)
        {
            RpcDoTimeBar();
            // This line does on the server what the next line does on the clients
            grid.activePlayer = activePlayer.GetComponent<Player>();
            RpcUpdateGridMenuActivePlayer();
        }
        // Update the gridMenu activePlayer object
        
    }

    private IEnumerator FlipArrow()
    {
        if (!turnArrow)
        {
            Debug.Log("MultiMan::Flip Arrow() - Arrow not set");
            yield return null;
        }
        for (int i = 0; i < 20; ++i)
        {
            turnArrow.transform.rotation = Quaternion.Lerp(turnArrow.transform.rotation, Quaternion.Euler(0, 0, 180 * turnCount), Time.deltaTime * 20);
            yield return new WaitForEndOfFrame();
        }
        justSwitched = false;
        yield return null;
    }

    /*
    public void PlaceUnit(GameObject location, UnitType type)
    {
        activePlayer.GetComponent<Player>().CmdPlaceUnit(location, type);
    }
    */

    private void SetupTurnQueue()
    {
        if (turnQueue == null) turnQueue = new Queue<GameObject>();
        if (player1 && player2)
        {
            if (player1GoesFirst)
            {
                activePlayer = player1;
                player1.GetComponent<Player>().activePlayer = true;
                Debug.Log("Player 1 is the active player!");
                turnQueue.Enqueue(player2);
                turnQueue.Enqueue(player1);
                turnCount = 1;
            }
            else
            {
                activePlayer = player2;
                player2.GetComponent<Player>().activePlayer = true;
                Debug.Log("Player 2 is the active player!");
                turnQueue.Enqueue(player1);
                turnQueue.Enqueue(player2);
            }
            StartCoroutine("FlipArrow");
        }
    }

    [ClientRpc]
    private void RpcUpdateGridMenuActivePlayer()
    {
        grid.activePlayer = activePlayer.GetComponent<Player>();
    }

    /*
    [ClientRpc]
    public void RpcPlaceUnit(GameObject location, UnitType type)
    {
        if (activePlayer) activePlayer.GetComponent<Player>().PlaceUnit(location, type);
        else Debug.Log("GameMan::PlaceUnit - Active Player not defined");
    }

    public void ReturnUnit(GameObject unit, GameObject owner)
    {
        if (owner.GetComponent<Player>())
            owner.GetComponent<Player>().ReturnUnit(unit);
    }
    */
    /*
    [Command]
    public void CmdDoTimeBar()
    {
        RpcDoTimeBar();
    }
    */

    [ClientRpc]
    public void RpcDoTimeBar()
    {
        //Debug.Log("Timebar Activated!");
        FindObjectOfType<TimeBar>().StartCoroutine("DoTimeBar");
    }

    public void SetActiveMenu(Menu activeMenu)
    {

    }
}
