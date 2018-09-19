using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_manager : MonoBehaviour
{
    public string playScene;
    public GameObject instructions;
    public GameObject playButton;
    public GameObject instructionsButton;
    public GameObject returnButton;

    private GameObject selectButton;


	// Use this for initialization
	void Start ()
    {
        returnButton.SetActive(false);
        instructions.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            SelectButton();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectButton == null)
                SelectButton();
            selectButton.GetComponent<Button>().onClick.Invoke();
        }
	}

    void SelectButton()
    {
        if (selectButton == null)
        {
            if (playButton.activeInHierarchy)
                selectButton = playButton;
            else
                selectButton = returnButton;
        }
        else if (selectButton == playButton)
            selectButton = instructionsButton;
        else if (selectButton == instructionsButton)
            selectButton = playButton;

        selectButton.GetComponent<Button>().Select();

    }

    public void PlayButton()
    {
        selectButton = null;
        SceneManager.LoadScene(playScene);
    }

    public void InstructionsButton()
    {
        selectButton = null;
        playButton.SetActive(false);
        instructionsButton.SetActive(false);
        returnButton.SetActive(true);
        instructions.SetActive(true);
    }

    public void ReturnButton()
    {
        selectButton = null;
        playButton.SetActive(true);
        instructionsButton.SetActive(true);
        returnButton.SetActive(false);
        instructions.SetActive(false);

    }
}
