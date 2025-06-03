using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Required for EventSystem
using UnityEngine.UI; // Required for Button component

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public Button firstPauseMenuButton;
    public string pauseInputButtonName = "PauseButton";
    public PlayerController playerController;

    void Start()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Update()
    {
        // Check for gamepad button press OR Escape key press
        if (Input.GetButtonDown("PauseButton") || Input.GetButtonDown(pauseInputButtonName))
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

        if (playerController != null)
        {
            playerController.enabled = true; // Re-enable the player's control script
            Debug.Log("PauseMenu: PlayerController enabled.");
        }

        // Clear the selected UI element when resuming
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("PauseMenu: Cleared selected GameObject as game resumes.");
        }
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0001f;
        GameIsPaused = true;

        if (playerController != null)
        {
            playerController.enabled = false; 
            Debug.Log("PauseMenu: PlayerController disabled.");
        }

        if (EventSystem.current != null && firstPauseMenuButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstPauseMenuButton.gameObject);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PauseMenuUI.SetActive(false);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("PauseMenu: PlayerController enabled on Restart.");
        }

        GameManager.Instance.ResetGame();
        GameObject gameplayGroup = GameObject.Find("GameplayGroup");
        GameObject mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        PlayerController player = FindObjectOfType<PlayerController>();
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        
        if (mainMenuCanvas != null && gameplayGroup != null && player != null && spawner != null)
        {
            mainMenuCanvas.SetActive(false);
            gameplayGroup.SetActive(true);
            player.ResetPlayer();
            spawner.ResetSpawner();
        }
        
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
