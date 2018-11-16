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

    public Menu activeMenu;
    public Menu prevMenu;

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
        Debug.Log(localPlayerAuthority);
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
            activePlayer = turnQueue.Dequeue();
            turnQueue.Enqueue(activePlayer);
            turnCount = ++turnCount % 2;
            StartCoroutine("FlipArrow");
        }
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.ResetPiece();
        }
        FindObjectOfType<TimeBar>().StopAllCoroutines();
        if(isServer) RpcDoTimeBar();
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

    public void HandleCrossButton(GameObject player)
    {
        if (player == activePlayer && isServer) RpcHandleCrossButton(player);
    }
    
    public void HandleCircleButton(GameObject player)
    {
        if (player == activePlayer && isServer) RpcHandleCircleButton(player);
    }
    
    public void HandleTriangleButton(GameObject player)
    {
        if (player == activePlayer && isServer) RpcHandleTriangleButton(player);
    }
    
    public void HandleSquareButton(GameObject player)
    {
        if (player == activePlayer && isServer) RpcHandleSquareButton(player);
    }
    
    public void PlaceUnit(GameObject location, UnitType type)
    {
        if(isServer) RpcPlaceUnit(location, type);
    }

    public void HandleHorizontalMovement(GameObject player, float horizontal)
    {
        if (player == activePlayer)
            activeMenu.HandleHorizontalMovement(horizontal);
    }

    public void HandleVerticalMovement(GameObject player, float vertical)
    {
        if (player == activePlayer)
            activeMenu.HandleVerticalMovement(vertical);
    }

    public void RpcHandleCrossButton(GameObject player)
    {
        activeMenu.HandleCrossButton();
    }

    public void RpcHandleTriangleButton(GameObject player)
    {
        if (!justSwitched)
            EndTurn();
    }

    public void RpcHandleCircleButton(GameObject player)
    {
        activeMenu.HandleCircleButton();
    }

    public void RpcHandleSquareButton(GameObject player)
    {
        activeMenu.HandleSquareButton();
    }

    /*
    [Command]
    public void CmdRegisterPlayer(GameObject player)
    {
        if (!player1)
        {
            player1 = player;
            Debug.Log("Found Player 1");
            player.GetComponent<Player>().identity = PlayerEnum.Player1;
        }
        else if (!player2)
        {
            player2 = player;
            Debug.Log("Found Player 2");
            player.GetComponent<Player>().identity = PlayerEnum.Player2;
        }
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
                Debug.Log("Player 1 is the active player!");
                turnQueue.Enqueue(player2);
                turnQueue.Enqueue(player1);
                turnCount = 1;
            }
            else
            {
                activePlayer = player2;
                Debug.Log("Player 2 is the active player!");
                turnQueue.Enqueue(player1);
                turnQueue.Enqueue(player2);
            }
            StartCoroutine("FlipArrow");
        }
    }

    public void SetActiveMenu(Menu newMenu)
    {
        prevMenu = activeMenu;
        activeMenu = newMenu;
    }

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
}
