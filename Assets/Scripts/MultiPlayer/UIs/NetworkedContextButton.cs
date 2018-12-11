using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkedContextButton : NetworkBehaviour
{
    public GameObject northNeighbor;
    public GameObject southNeighbor;
    public NetworkedGridMenu gridMenu;

    private bool alreadyDone = false;

    public ButtonTypeEnum buttonType;

    public bool locked = false;
    // TO-DO - MAKE MENU HANDLE THINGS BEING LOCKED

    private void OnEnable()
    {
        if (alreadyDone) return;
        gridMenu = FindObjectOfType<NetworkedGridMenu>();
        if(buttonType != ButtonTypeEnum.None) gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        switch (buttonType)
        {
            case ButtonTypeEnum.SpawnUnit:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { gridMenu.PlaceUnit("unit"); });
                break;
            case ButtonTypeEnum.SpawnPusher:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { gridMenu.PlaceUnit("pusher"); });
                break;
            case ButtonTypeEnum.SpawnPuller:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { gridMenu.PlaceUnit("puller"); });
                break;
            case ButtonTypeEnum.SpawnTwister:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { gridMenu.PlaceUnit("twister"); });
                break;
            case ButtonTypeEnum.SpawnPortalPlacer:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { gridMenu.PlaceUnit("portalPlacer"); });
                break;
            case ButtonTypeEnum.Move:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { GetComponentInParent<NetworkedUnit>().DisplayMoveGrid(); });
                break;
            case ButtonTypeEnum.Push:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { GetComponentInParent<NetworkedPusher>().DisplayActionGrid(); });
                break;
            case ButtonTypeEnum.Pull:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { GetComponentInParent<NetworkedPuller>().DisplayActionGrid(); });
                break;
            case ButtonTypeEnum.Twist:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { GetComponentInParent<NetworkedTwister>().DisplayActionGrid(); });
                break;
        }
        alreadyDone = true;
    }
}