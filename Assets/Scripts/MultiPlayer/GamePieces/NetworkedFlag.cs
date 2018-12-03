using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedFlag : NetworkedGamePiece
{
    // Use this for initialization
    void Start()
    {
        FindGridElement();
    }

    void Update()
    {
        if (!gridElement)
            FindGridElement();
    }
}
