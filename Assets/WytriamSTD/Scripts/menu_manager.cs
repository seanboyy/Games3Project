using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_manager : Menu
{
    public string playScene;
    public GameObject instructions;
    public GameObject playButton;
    public GameObject instructionsButton;
    public GameObject returnButton;

    private bool canPressButtons = false;


    // Update is called once per frame
    void Update ()
    {
        if (activeUIMenu)
        {
            // Move left or move up
            if ((prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") < 0) || (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") > 0))
            {
                SelectElement(selectedGO.GetComponent<ContextButton>().northNeighbor);
            }
            // Move right or move down
            else if ((prevHorAxis == 0 && Input.GetAxisRaw("Horizontal") > 0) || (prevVerAxis == 0 && Input.GetAxisRaw("Vertical") < 0))
            {
                SelectElement(selectedGO.GetComponent<ContextButton>().southNeighbor);
            }

            prevHorAxis = Input.GetAxisRaw("Horizontal");
            prevVerAxis = Input.GetAxisRaw("Vertical");

            if (canPressButtons)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    selectedGO.GetComponent<Button>().onClick.Invoke();
                }

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
                    //Cancel();
            }
            else
                canPressButtons = true;
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(playScene);
    }

    public void InstructionsButton()
    {
        //selectButton = null;
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        returnButton.SetActive(true);
        instructions.SetActive(true);
    }

    public void ReturnButton()
    {
        //selectButton = null;
        playButton.SetActive(true);
        instructionsButton.SetActive(true);
        returnButton.SetActive(false);
        instructions.SetActive(false);
    }
}
