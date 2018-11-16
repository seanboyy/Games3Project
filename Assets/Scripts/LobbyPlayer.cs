using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkBehaviour {

    public LobbyMan lobbyMan;

	// Use this for initialization
	void Start () {
        lobbyMan = FindObjectOfType<LobbyMan>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
