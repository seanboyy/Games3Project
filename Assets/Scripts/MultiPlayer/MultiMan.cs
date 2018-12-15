﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MultiMan : NetworkBehaviour, IGameMan
{
    public Sprite[] pullerSprites;
    public Sprite[] pusherSprites;
    public Sprite[] twisterSprites;
    public Sprite[] portalPlacerSprites;
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
    public NetworkedGridMenu grid;

    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            player1GoesFirst = Random.Range(0F, 1F) < 0.5F;
            RpcDoTimeBar();
        }
    }

    public void RegisterPlayers()
    {
        if (isServer)
        {
            if (FindObjectsOfType<NetworkedPlayer>().Length < 2) return;
            player1 = FindObjectsOfType<NetworkedPlayer>()[0].gameObject;
            player1.name = "Player1";
            player1.GetComponent<NetworkedPlayer>().identity = PlayerEnum.Player1;
            player2 = FindObjectsOfType<NetworkedPlayer>()[1].gameObject;
            player2.GetComponent<NetworkedPlayer>().identity = PlayerEnum.Player2;
            player2.name = "Player2";
            player1.GetComponent<NetworkedPlayer>().gameManager = this;
            player2.GetComponent<NetworkedPlayer>().gameManager = this;
            player1.GetComponent<NetworkedPlayer>().gameManager.player1GoesFirst = player1GoesFirst;
            player2.GetComponent<NetworkedPlayer>().gameManager.player1GoesFirst = player1GoesFirst;
            if (player1GoesFirst)
            {
                player1.GetComponent<NetworkedPlayer>().gameManager.activePlayer = player1;
                player2.GetComponent<NetworkedPlayer>().gameManager.activePlayer = player1;
                RpcSetupTurnQueue();
            }
            else
            {
                player1.GetComponent<NetworkedPlayer>().gameManager.activePlayer = player2;
                player2.GetComponent<NetworkedPlayer>().gameManager.activePlayer = player2;
                RpcSetupTurnQueue();
            }
            player1.GetComponent<NetworkedPlayer>().gameManager.SetupTurnQueue();
            player2.GetComponent<NetworkedPlayer>().gameManager.SetupTurnQueue();
        }
        /*
        if (player1) player1.GetComponent<NetworkedPlayer>().gameManager.player1.name = "Player1";
        if (player1) player1.GetComponent<NetworkedPlayer>().gameManager.player2.name = "Player2";
        if (player2) player2.GetComponent<NetworkedPlayer>().gameManager.player1.name = "Player1";
        if (player2) player2.GetComponent<NetworkedPlayer>().gameManager.player2.name = "Player2";
        */
        if (player1) player1.name = "Player1";
        if (player2) player2.name = "Player2";
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
        if (!justSwitched)
        {
            if (player1 && player2)
            {
                justSwitched = true;
                activePlayer.GetComponent<NetworkedPlayer>().activePlayer = false;
                activePlayer = turnQueue.Dequeue();
                activePlayer.GetComponent<NetworkedPlayer>().activePlayer = true;
                turnQueue.Enqueue(activePlayer);
                turnCount = ++turnCount % 2;
                // Reason the arrow doesn't flip in sync is because this isn't an RPC
                StartCoroutine("FlipArrow");
            }
            foreach (NetworkedUnit unit in FindObjectsOfType<NetworkedUnit>())
            {
                unit.ResetPiece();
            }
            FindObjectOfType<TimeBar>().StopAllCoroutines();
            grid.activePlayer = activePlayer.GetComponent<NetworkedPlayer>();
            if (isServer)
            {
                RpcDoTimeBar();
                // This line does on the server what the next line does on the clients
            }
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

    [ClientRpc]
    public void RpcSetupTurnQueue()
    {
        SetupTurnQueue();
    }

    private void SetupTurnQueue()
    {
        if (turnQueue == null) turnQueue = new Queue<GameObject>();
        if (player1 && player2)
        {
            if (player1GoesFirst)
            {
                activePlayer = player1;
                grid.activePlayer = activePlayer.GetComponent<NetworkedPlayer>();
                player1.GetComponent<NetworkedPlayer>().activePlayer = true;
                turnQueue.Enqueue(player2);
                turnQueue.Enqueue(player1);
                turnCount = 1;
            }
            else
            {
                activePlayer = player2;
                grid.activePlayer = activePlayer.GetComponent<NetworkedPlayer>();
                player2.GetComponent<NetworkedPlayer>().activePlayer = true;
                turnQueue.Enqueue(player1);
                turnQueue.Enqueue(player2);
            }
            StartCoroutine("FlipArrow");
        }
    }

    [ClientRpc]
    private void RpcUpdateGridMenuActivePlayer()
    {
        grid.activePlayer = activePlayer.GetComponent<NetworkedPlayer>();
    }

    [ClientRpc]
    public void RpcDoTimeBar()
    {
        //Debug.Log("Timebar Activated!");
        FindObjectOfType<TimeBar>().StartCoroutine("DoTimeBar");
    }
}
