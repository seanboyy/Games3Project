using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedContextButton : NetworkBehaviour
{
    public GameObject northNeighbor;
    public GameObject southNeighbor;

    public bool locked = false;
    // TO-DO - MAKE MENU HANDLE THINGS BEING LOCKED
}