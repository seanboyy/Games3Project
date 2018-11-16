using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkBehaviour {

    public static NetworkLobbyPlayer lobbyPlayer;
    public NetworkLobbyManager lobbyManager;
    public LobbyMan lobbyMan;

	// Use this for initialization
	void Start () {
        lobbyMan = FindObjectOfType<LobbyMan>();
        if (isLocalPlayer) lobbyPlayer = GetComponent<NetworkLobbyPlayer>();
	}

	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) (lobbyManager.OnLobbyServerCreateGamePlayer(connectionToClient, playerControllerId)).GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
	}
}
