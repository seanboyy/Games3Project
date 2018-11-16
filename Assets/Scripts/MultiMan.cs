using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MultiMan : NetworkBehaviour, IGameMan
{

    public string nextSceneName;
    public GameObject activePlayer;

    public Menu activeMenu;
    public Menu prevMenu;

    public GameObject player1;
    public GameObject player2;
    public Queue<GameObject> turnQueue;
    public Image turnArrow;
    private int turnCount = 0;
    private bool player1GoesFirst = true;
    private bool justSwitched = false;

    // Use this for initialization
    void Start()
    {
        if (isServer) Debug.Log(gameObject.name + " is the server!");
        else Debug.Log(gameObject.name + " is not the server!");
        if(isServer) RpcDoTimeBar();
        if (isServer) player1GoesFirst = Random.Range(0F, 1F) < 0.5F;
    }

    // Update is called once per frame
    void Update() { }

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
        if (isServer)
        {
            Debug.Log("Telling players to do timeBar");
            RpcDoTimeBar();
        }
        else
        {
            Debug.Log("Telling server to tell players to do timeBar");
            CmdDoTimeBar();
        }
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

    [Command]
    public void CmdHandleCrossButton(bool local, GameObject player)
    {
        if (local && player == activePlayer) RpcHandleCrossButton(player);
    }

    [Command]
    public void CmdHandleCircleButton(bool local, GameObject player)
    {
        if (local && player == activePlayer) RpcHandleCircleButton(player);
    }

    [Command]
    public void CmdHandleTriangleButton(bool local, GameObject player)
    {
        if (local && player == activePlayer) RpcHandleTriangleButton(player);
    }

    [Command]
    public void CmdHandleSquareButton(bool local, GameObject player)
    {
        if (local && player == activePlayer) RpcHandleSquareButton(player);
    }

    [Command]
    public void CmdPlaceUnit(GameObject location, UnitType type)
    {
        RpcPlaceUnit(location, type);
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
        if (player == activePlayer)
            activeMenu.HandleCrossButton();
    }

    public void RpcHandleTriangleButton(GameObject player)
    {
        if (player == activePlayer && !justSwitched)
            EndTurn();
    }

    public void RpcHandleCircleButton(GameObject player)
    {
        if (player == activePlayer)
            activeMenu.HandleCircleButton();
    }

    public void RpcHandleSquareButton(GameObject player)
    {
        if (player == activePlayer)
            activeMenu.HandleSquareButton();
    }

    [Command]
    public void CmdRegisterPlayer(GameObject player)
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
    }

    [ClientRpc]
    private void RpcSetupTurnQueue()
    {
        if (turnQueue == null) turnQueue = new Queue<GameObject>();
        if (player1 && player2)
        {
            if (turnQueue.Count < 2)
                if (player1GoesFirst)
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

    [Command]
    public void CmdDoTimeBar()
    {
        RpcDoTimeBar();
    }

    [ClientRpc]
    public void RpcDoTimeBar()
    {
        //Debug.Log("Timebar Activated!");
        FindObjectOfType<TimeBar>().StartCoroutine("DoTimeBar");
    }

    public void HandleCrossButton(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public void HandleTriangleButton(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public void HandleCircleButton(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public void HandleSquareButton(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public void PlaceUnit(GameObject location, UnitType type)
    {
        throw new System.NotImplementedException();
    }
}
