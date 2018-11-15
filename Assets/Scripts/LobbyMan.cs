using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LobbyMan : Menu {

    float prevHorAxis = 0F;
    float prevVerAxis = 0F;

    public bool flag = false;

    public GameObject lobbyManagerPrefab;

    public NetworkLobbyManager lobbyManager;

    // Use this for initialization
    protected override void Start () {
        NetworkLobbyManager temp = Resources.FindObjectsOfTypeAll<NetworkLobbyManager>()[0];
        lobbyManager = !EditorUtility.IsPersistent(temp.gameObject.transform.root) ? temp : null;
        if (!lobbyManager)
        {
            lobbyManager = Instantiate(lobbyManagerPrefab).GetComponent<NetworkLobbyManager>();
        }
        else
        {
            lobbyManager.gameObject.SetActive(true);
        }
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") != 0)
            HandleHorizontalMovement(Input.GetAxisRaw("Horizontal"));
        if (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") != 0)
            HandleVerticalMovement(Input.GetAxisRaw("Vertical"));

        prevHorAxis = Input.GetAxisRaw("Horizontal");
        prevVerAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            HandleCrossButton();
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            HandleCircleButton();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            HandleTriangleButton();
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            HandleSquareButton();
    }

    public void LoadMap0()
    {
        lobbyManager.playScene = Statics.multiplayerScenes[0];
    }

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

    public override void HandleHorizontalMovement(float horizontal)
    {
        if (horizontal > 0)
        {
            SelectElement(selectedGO.GetComponent<ContextButton>().southNeighbor);
        }
        else
        {
            SelectElement(selectedGO.GetComponent<ContextButton>().northNeighbor);
        }
    }

    public override void HandleVerticalMovement(float vertical)
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

    public override void HandleCrossButton()
    {
        selectedGO.GetComponent<Button>().onClick.Invoke();
    }

    public override void HandleTriangleButton()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleCircleButton()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleSquareButton()
    {
        throw new System.NotImplementedException();
    }
}
