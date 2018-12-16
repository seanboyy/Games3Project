using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyMan : MonoBehaviour
{

    float prevHorAxis = 0F;
    float prevVerAxis = 0F;

    public bool flag = false;

    public GameObject lobbyManagerPrefab;

    public NetworkLobbyManager lobbyManager;

    [Header("Colors for UI Elements")]
    public static Color defaultColor = Color.white;   // the color for non-selected Buttons
    public static Color selectedColor = Color.cyan;  // the color for non-active, selected UI elements

    public GameObject selectedGO;

    [Header("Is this menu under active player control?")]
    public bool activeUIMenu = false;

    protected Color prevColor;    // the color of the previous selectedElement
    protected Menu prevMenu;


    private void Awake()
    {
        if (lobbyManager)
        {
            Statics.lobbyManager = lobbyManager;
        }
        else
        {
            lobbyManager = Statics.lobbyManager;
        }
    }

    // Use this for initialization
    protected void Start()
    {
        SelectElement(selectedGO);
        prevColor = defaultColor;
    }

    private void OnEnable()
    {
        if (!Statics.lobbyManager) Statics.lobbyManager = lobbyManager;
    }

    protected virtual void SelectElement(GameObject newElement)
    {
        if (newElement == null) return;
        selectedGO.GetComponent<Image>().color = prevColor;
        selectedGO = newElement;
        prevColor = selectedGO.GetComponent<Image>().color;
        selectedGO.GetComponent<Image>().color = selectedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            HandleHorizontalMovement(Input.GetAxisRaw("Horizontal"));
        if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            HandleVerticalMovement(Input.GetAxisRaw("Vertical"));

        prevHorAxis = Input.GetAxisRaw("Horizontal");
        prevVerAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            HandleCrossButton();
    }

    public void LoadMap(int mapNum)
    {
        Debug.Log(Statics.multiplayerScenes[mapNum]);
        SetScene(Statics.multiplayerScenes[mapNum]);
        //else RpcSetScene(Statics.multiplayerScenes[mapNum]);
    }

    public void SetScene(string sceneName)
    {
        lobbyManager.playScene = sceneName;
    }

    /*
    [Command]
    public void CmdSetScene(string sceneName)
    {
        RpcSetScene(sceneName);
    }

    [ClientRpc]
    public void RpcSetScene(string sceneName)
    {
        lobbyManager.playScene = sceneName;
    }
    */

    public void ToMenu()
    {
        foreach (NetworkLobbyPlayer player in FindObjectsOfType<NetworkLobbyPlayer>())
        {
            Destroy(player.gameObject);
        }
        StartCoroutine("GotoMenu");
    }

    private IEnumerator GotoMenu()
    {
        yield return new WaitForSeconds(0.1F);
        lobbyManager.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1F);
        SceneManager.LoadScene("menu");
        yield return null;
    }

    public void HandleHorizontalMovement(float horizontal)
    {
        if (horizontal > 0)
        {
            SelectElement(selectedGO.GetComponent<_2DContextButton>().westNeighbor);
        }
        else
        {
            SelectElement(selectedGO.GetComponent<_2DContextButton>().eastNeighbor);
        }
    }

    public void HandleVerticalMovement(float vertical)
    {
        if (vertical > 0)
        {
            SelectElement(selectedGO.GetComponent<ContextButton>().northNeighbor);
        }
        else
        {
            SelectElement(selectedGO.GetComponent<ContextButton>().southNeighbor);
        }
    }

    public void HandleCrossButton()
    {
        selectedGO.GetComponent<Button>().onClick.Invoke();
    }
}
