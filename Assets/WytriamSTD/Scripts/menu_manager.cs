using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class menu_manager : Menu
{
    public string playScene;
    public GameObject instructions;
    public GameObject playButton;
    public GameObject multiplayButton;
    public GameObject instructionsButton;
    public GameObject returnButton;
    

    private float prevHorAxis = 0;
    private float prevVerAxis = 0;

    protected override void Start()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach(Player player in players)
        {
            Destroy(player.gameObject);
        }
        base.Start();
        returnButton.SetActive(false);
        instructions.SetActive(false);
        activeUIMenu = true;
    }

    // We're being hacky with this menu and pretending that like a player, it can get input
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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            HandleCircleButton();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            HandleTriangleButton();
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton3))
            HandleSquareButton();
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
        if (selectedGO == returnButton)
            ReturnButton();
    }

    public override void HandleSquareButton()
    {
        throw new System.NotImplementedException();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(playScene);
    }

    public void MultiplayerPlayButton()
    {
        SceneManager.LoadScene("lobby");
    }

    public void InstructionsButton()
    {
        SelectElement(returnButton);
        playButton.SetActive(false);
        multiplayButton.SetActive(false);
        instructionsButton.SetActive(false);
        returnButton.SetActive(true);
        instructions.SetActive(true);
    }

    public void ReturnButton()
    {
        SelectElement(instructionsButton);
        playButton.SetActive(true);
        multiplayButton.SetActive(true);
        instructionsButton.SetActive(true);
        returnButton.SetActive(false);
        instructions.SetActive(false);
    }
}
