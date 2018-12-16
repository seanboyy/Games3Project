using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class NetworkMan : NetworkLobbyManager
{
    public LobbyMan lobbyMan;
    public Button findMatchButton;
    public Button mapButton;
    public Button menuButton;
    public Button addPlayerButton;
    public Button cancelButton;


    // Use this for initialization
    void Start()
    {
        Cancel();
        Statics.lobbyManager = this;
        singleton.StartMatchMaker();
        maxPlayersPerConnection = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectsOfType<LobbyPlayer>().Length > 0) FindObjectsOfType<LobbyPlayer>()[0].gameObject.transform.position = new Vector3(-3, 0, 0);
        if (FindObjectsOfType<LobbyPlayer>().Length > 1) FindObjectsOfType<LobbyPlayer>()[1].gameObject.transform.position = new Vector3(3, 0, 0);
    }

    public void FindMatch()
    {
        findMatchButton.gameObject.SetActive(false);
        addPlayerButton.gameObject.SetActive(true);
        singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnInternetMatchList);
    }

    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
            if (matches.Count != 0)
            {
                if (matches[matches.Count - 1].currentSize < 2) singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
                else matchMaker.CreateMatch("", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
            }
            else
            {
                matchMaker.CreateMatch("", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
            }
    }

    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);
            singleton.StartHost(hostInfo);
        }
    }

    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            MatchInfo hostInfo = matchInfo;
            singleton.StartClient(hostInfo);
        }
        else
        {
            matchMaker.CreateMatch("", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
        }
    }
    public void Cancel()
    {
        findMatchButton.gameObject.SetActive(true);
        addPlayerButton.gameObject.SetActive(false);
    }

    public void AddPlayer()
    {
        ClientScene.AddPlayer(1);
        FindMatch();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        LobbyPlayer[] players = FindObjectsOfType<LobbyPlayer>();
        if(players.Length == 2)
        {
            if(players[0].connectionToServer == players[1].connectionToServer)
            {
                print("2 players on this connection");
            }
        }
        base.OnServerAddPlayer(conn, playerControllerId);
    }
}
