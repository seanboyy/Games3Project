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
    bool position1Set = false;
    bool position2Set = false;


    // Use this for initialization
    void Start()
    {
        Cancel();
        singleton.StartMatchMaker();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lobbyMan.gameObject.activeInHierarchy) lobbyMan.gameObject.SetActive(true);
        if (!position1Set)
        {
            if (FindObjectsOfType<LobbyPlayer>().Length > 0)
            {
                FindObjectsOfType<LobbyPlayer>()[0].gameObject.transform.position = new Vector3(-3, 0, 0);
                position1Set = true;
            }
        }
        if (!position2Set)
        {
            if(FindObjectsOfType<LobbyPlayer>().Length > 1)
            {
                FindObjectsOfType<LobbyPlayer>()[1].gameObject.transform.position = new Vector3(3, 0, 0);
                position2Set = true;
            }
        }
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
        {
            if (matches.Count != 0)
            {
                Debug.Log("Found Match! Joining!");
                singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
            }
            else
            {
                Debug.Log("Could not find a match! Creating!");
                matchMaker.CreateMatch("", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
            }
        }
    }

    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Created Internet Match!");
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);
            singleton.StartHost(hostInfo);
        }
    }

    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Joined Internet Match!");
            MatchInfo hostInfo = matchInfo;
            singleton.StartClient(hostInfo);
            Debug.Log(playScene);
        }
        else
        {
            Debug.Log("Could not join match. Creating!");
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
        FindMatch();
    }
}
