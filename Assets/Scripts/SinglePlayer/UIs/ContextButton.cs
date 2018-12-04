using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextButton : MonoBehaviour
{
    public GameObject northNeighbor;
    public GameObject southNeighbor;
    public GridMenu gridMenu;

    public ButtonTypeEnum buttonType;

    public bool locked = false;
    // TO-DO - MAKE MENU HANDLE THINGS BEING LOCKED

    private void Start()
    {
        gridMenu = FindObjectOfType<GridMenu>();
        if (buttonType != ButtonTypeEnum.None) gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
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
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { transform.root.gameObject.GetComponent<Unit>().ActivateMoveButton(); });
                break;
            case ButtonTypeEnum.Push:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { transform.root.gameObject.GetComponent<Pusher>().ActivatePushButton(); });
                break;
            case ButtonTypeEnum.Pull:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { transform.root.gameObject.GetComponent<Puller>().ActivatePullButton(); });
                break;
            case ButtonTypeEnum.Twist:
                gameObject.GetComponent<Button>().onClick.AddListener(delegate { transform.root.gameObject.GetComponent<Twister>().ActivateTwistButton(); });
                break;
        }
    }
}