using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    public GameObject PauseMenuUI;

    void Start()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void  RestartGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PauseMenuUI.SetActive(false);

        // Reset game logic
        GameManager.Instance.ResetGame();

        // Reset gameplay UI state
        GameObject mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false); // Hide main menu
        }

        GameObject gameplayGroup = GameObject.Find("GameplayGroup"); // or reference it via serialized field
        if (gameplayGroup != null)
        {
            gameplayGroup.SetActive(true); // Show gameplay elements
        }
        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ResetPlayer();
        }
    }


    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}