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

    public void FindMatch()
    {
        findMatchButton.gameObject.SetActive(false);
        addPlayerButton.gameObject.SetActive(true);
        // Turn off all the references to this findmatch button
        _2DContextButton findButton = findMatchButton.GetComponent<_2DContextButton>();
        if (findButton.northNeighbor) findButton.northNeighbor.GetComponent<_2DContextButton>().southNeighbor = null;
        if (findButton.southNeighbor) findButton.southNeighbor.GetComponent<_2DContextButton>().northNeighbor = null;
        if (findButton.westNeighbor) findButton.westNeighbor.GetComponent<_2DContextButton>().eastNeighbor = null;
        if (findButton.eastNeighbor) findButton.eastNeighbor.GetComponent<_2DContextButton>().westNeighbor = null;

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
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {

        base.OnLobbyClientConnect(conn);
    }

    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
    }

    public override void OnLobbyServerSceneChanged(string sceneName)
    {
        base.OnLobbyServerSceneChanged(sceneName);
        foreach(LobbyPlayer player in FindObjectsOfType<LobbyPlayer>())
        {
            player.readyButton.gameObject.SetActive(false);
            player.notReadyButton.gameObject.SetActive(false);
        }
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }
}
