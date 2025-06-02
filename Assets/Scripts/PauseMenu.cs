using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Required for EventSystem
using UnityEngine.UI; // Required for Button component

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;
    public GameObject PauseMenuUI;

    // Assign the first button in your pause menu (e.g., "Resume Button") here in the Inspector
    public Button firstPauseMenuButton;

    // Name of the input button to open/close the pause menu (e.g., "Cancel", "StartButton")
    public string pauseInputButtonName = "PauseButton";

    // --- NEW: Reference to your PlayerController script ---
    // Drag your Player GameObject (or the GameObject with PlayerController) here in the Inspector
    public PlayerController playerController;

    void Start()
    {
        // Ensure the pause menu is hidden and game is running at start
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // --- NEW: Initial check for PlayerController reference ---
        if (playerController == null)
        {
            // Try to find it if not assigned (less efficient, but a fallback)
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("PauseMenu: PlayerController reference is not assigned in the Inspector and could not be found in the scene! Player input will not be disabled/enabled correctly.");
            }
        }
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

        // --- NEW: Enable Player Input ---
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

        // --- NEW: Disable Player Input ---
        if (playerController != null)
        {
            playerController.enabled = false; // Disable the player's control script
            Debug.Log("PauseMenu: PlayerController disabled.");
        }

        // Set the initially selected UI element for gamepad navigation
        if (EventSystem.current != null && firstPauseMenuButton != null)
        {
            if (!firstPauseMenuButton.gameObject.activeInHierarchy)
            {
                Debug.LogWarning("PauseMenu: firstPauseMenuButton is assigned but its GameObject is not active in the hierarchy!");
            }
            EventSystem.current.SetSelectedGameObject(firstPauseMenuButton.gameObject);
            Debug.Log("PauseMenu: First Pause Menu Button set as initial selected GameObject for EventSystem.");
        }
        else
        {
            if (EventSystem.current == null)
            {
                Debug.LogError("PauseMenu: EventSystem not found when trying to select pause menu button! Please ensure you have an EventSystem GameObject.");
            }
            if (firstPauseMenuButton == null)
            {
                Debug.LogError("PauseMenu: First Pause Menu Button is not assigned in the Inspector!");
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PauseMenuUI.SetActive(false);

        // Ensure EventSystem selected object is null before restarting
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        // --- NEW: Re-enable Player Input on Restart if it's still disabled ---
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("PauseMenu: PlayerController enabled on Restart.");
        }

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
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.ResetSpawner();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
