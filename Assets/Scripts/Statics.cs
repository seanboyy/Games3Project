using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Statics
{
    public static readonly string[] singleplayerScenes = { "level-1", "level-2", "level-3", "level-4" };
    public static readonly string[] multiplayerScenes = { "multi-map-1" };
    public static int selectedScene = 0;
    public static NetworkLobbyManager lobbyManager;
}