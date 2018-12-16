using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour {

    public NetworkLobbyPlayer lobbyPlayer;
    public LobbyMan lobbyMan;
    public NetworkMan networkMan;
    public Button readyButton;
    public Button notReadyButton;

    // This is to make the menu work nice
    public bool multiplePlayers = false;
    private bool playerOnRight;
    private LobbyPlayer otherPlayer;
    [HideInInspector]
    public GameObject activeButton;

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        // Find any other player objects and their position relative to this player 
        LobbyPlayer[] players = FindObjectsOfType<LobbyPlayer>();
        if (players.Length > 1)
        {
            multiplePlayers = true;
            foreach (LobbyPlayer player in players)
            {
                if (player == this) continue;
                if (player.transform.position.x >= transform.position.x)
                    playerOnRight = true;
                else
                    playerOnRight = false;
                otherPlayer = player;
            }
        }
        if (isLocalPlayer)
        {
            lobbyMan = FindObjectOfType<LobbyMan>();
            lobbyPlayer = GetComponent<NetworkLobbyPlayer>();
            networkMan = FindObjectOfType<NetworkMan>();
            SetUpButton(readyButton.GetComponent<_2DContextButton>());
        }
        else
        {
            readyButton.enabled = false;
            notReadyButton.enabled = false;
        }
    }

    void SetUpButton(_2DContextButton button)
    {
        activeButton = button.gameObject;
        if (multiplePlayers && otherPlayer)
        {
            if (playerOnRight)
            {
                button.eastNeighbor = otherPlayer.activeButton;
                if (button.eastNeighbor) button.eastNeighbor.GetComponent<_2DContextButton>().westNeighbor = button.gameObject;
            }
            else
            {
                button.westNeighbor = activeButton;
                if (button.westNeighbor) button.westNeighbor.GetComponent<_2DContextButton>().eastNeighbor = button.gameObject;
            }
        }

        button.northNeighbor = lobbyMan.addPlayerButton;
        if (button.northNeighbor) button.northNeighbor.GetComponent<_2DContextButton>().southNeighbor = button.gameObject;

        button.southNeighbor = lobbyMan.returnToMenuButton;
        if (button.southNeighbor) button.southNeighbor.GetComponent<_2DContextButton>().northNeighbor = button.gameObject;
     
        // Select this button
        lobbyMan.SelectElement(button.gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (isLocalPlayer && networkMan && networkMan.OnLobbyServerCreateGamePlayer(connectionToServer, playerControllerId))
            networkMan.OnLobbyServerCreateGamePlayer(connectionToServer, playerControllerId).GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToServer);
	}

    public void Ready()
    {
        SetUpButton(notReadyButton.GetComponent<_2DContextButton>());
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
        SetUpButton(readyButton.GetComponent<_2DContextButton>());
        lobbyPlayer.SendNotReadyToBeginMessage();
        notReadyButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(true);
        networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().westNeighbor = readyButton.gameObject;
        networkMan.mapButton.gameObject.GetComponent<_2DContextButton>().eastNeighbor = readyButton.gameObject;
        networkMan.addPlayerButton.GetComponent<_2DContextButton>().northNeighbor = readyButton.gameObject;
        networkMan.addPlayerButton.GetComponent<_2DContextButton>().southNeighbor = readyButton.gameObject;
    }
}
