using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour {

    public static NetworkLobbyPlayer lobbyPlayer;
    public NetworkLobbyManager lobbyManager;
    public LobbyMan lobbyMan;
    public NetworkMan networkMan;
    public Button readyButton;
    public Button notReadyButton;

	// Use this for initialization
	void Start () {
        lobbyMan = FindObjectOfType<LobbyMan>();
        lobbyManager = FindObjectOfType<NetworkLobbyManager>();
        networkMan = FindObjectOfType<NetworkMan>();
        readyButton.gameObject.SetActive(true);
        notReadyButton.gameObject.SetActive(false);
	}

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            lobbyPlayer = GetComponent<NetworkLobbyPlayer>();
            networkMan = FindObjectOfType<NetworkMan>();
            readyButton.gameObject.GetComponent<_2DContextButton>().eastNeighbor = networkMan.mapButton.gameObject;
            readyButton.gameObject.GetComponent<_2DContextButton>().westNeighbor = networkMan.mapButton.gameObject;
            readyButton.gameObject.GetComponent<_2DContextButton>().northNeighbor = networkMan.addPlayerButton.gameObject;
            readyButton.gameObject.GetComponent<_2DContextButton>().southNeighbor = networkMan.addPlayerButton.gameObject;
            notReadyButton.gameObject.GetComponent<_2DContextButton>().eastNeighbor = networkMan.mapButton.gameObject;
            notReadyButton.gameObject.GetComponent<_2DContextButton>().westNeighbor = networkMan.mapButton.gameObject;
            notReadyButton.gameObject.GetComponent<_2DContextButton>().southNeighbor = networkMan.addPlayerButton.gameObject;
            notReadyButton.gameObject.GetComponent<_2DContextButton>().northNeighbor = networkMan.addPlayerButton.gameObject;
            networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().westNeighbor = readyButton.gameObject;
            networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().eastNeighbor = readyButton.gameObject;
            networkMan.addPlayerButton.GetComponent<_2DContextButton>().northNeighbor = readyButton.gameObject;
            networkMan.addPlayerButton.GetComponent<_2DContextButton>().southNeighbor = readyButton.gameObject;
        }
        else
        {
            readyButton.enabled = false;
            notReadyButton.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
        if (isLocalPlayer && lobbyManager && lobbyManager.OnLobbyServerCreateGamePlayer(connectionToServer, playerControllerId)) (lobbyManager.OnLobbyServerCreateGamePlayer(connectionToServer, playerControllerId)).GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToServer);
	}

    public void Ready()
    {
        lobbyPlayer.SendReadyToBeginMessage();
        readyButton.gameObject.SetActive(false);
        notReadyButton.gameObject.SetActive(true);
        networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().westNeighbor = notReadyButton.gameObject;
        networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().eastNeighbor = notReadyButton.gameObject;
        networkMan.addPlayerButton.GetComponent<_2DContextButton>().northNeighbor = notReadyButton.gameObject;
        networkMan.addPlayerButton.GetComponent<_2DContextButton>().southNeighbor = notReadyButton.gameObject;
    }

    public void NotReady()
    {
        lobbyPlayer.SendNotReadyToBeginMessage();
        notReadyButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(true);
        networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().westNeighbor = readyButton.gameObject;
        networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().eastNeighbor = readyButton.gameObject;
        networkMan.addPlayerButton.GetComponent<_2DContextButton>().northNeighbor = readyButton.gameObject;
        networkMan.addPlayerButton.GetComponent<_2DContextButton>().southNeighbor = readyButton.gameObject;
    }

}
