using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button firstSelectedButton;
    private static bool GameOver = false;

    void Awake()
    {
        // Ensure GameOverUI is initially hidden and time scale is normal
        GameOverUI.SetActive(false);
        Time.timeScale = 1f;
        GameOver = false;
    }

    /// <summary>
    /// Displays the Game Over UI, pauses the game, and sets the score.
    /// </summary>
    public void ShowGameOver()
    {
        GameOverUI.SetActive(true); // Activate the Game Over UI
        Time.timeScale = 0f;        // Pause the game
        GameOver = true;            // Set game over state

        // Display the player's score if GameManager and scoreText are available
        if (GameManager.Instance != null && scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.playerScore}";
            Debug.Log($"GameOverMenu: Showing Game Over Score: {GameManager.Instance.playerScore}");
        }
        else
        {
            Debug.LogWarning("GameOverMenu: GameManager.Instance or scoreText is null. Cannot display score.");
        }

        // Select the first button for gamepad navigation
        StartCoroutine(SelectFirstButton());
    }

    /// <summary>
    /// Coroutine to select the first button for UI navigation after a frame delay.
    /// </summary>
    private IEnumerator SelectFirstButton()
    {
        yield return null; // Wait one frame for UI to initialize
        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
            Debug.Log("GameOverMenu: First button selected for gamepad input.");
        }
        else
        {
            Debug.LogWarning("GameOverMenu: EventSystem or firstSelectedButton is null. Cannot select first button.");
        }
    }

    /// <summary>
    /// Restarts the game, hiding the Game Over UI and resetting game elements.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;        // Resume game time
        GameOver = false;           // Reset game over state
        GameOverUI.SetActive(false); // Hide Game Over UI

        // Reset game state via GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
            Debug.Log("GameOverMenu: Game state reset via GameManager.");
        }
        else
        {
            Debug.LogError("GameOverMenu: GameManager.Instance is null. Cannot reset game.");
        }

        // Reactivate gameplay group
        GameObject gameplayGroup = GameObject.Find("GamePlay");
        if (gameplayGroup != null)
        {
            gameplayGroup.SetActive(true);
            Debug.Log("GameOverMenu: GameplayGroup activated.");
        }
        else
        {
            Debug.LogWarning("GameOverMenu: GameplayGroup not found.");
        }

        // Reset player controller
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ResetPlayer();
            Debug.Log("GameOverMenu: Player reset.");
        }
        else
        {
            Debug.LogWarning("GameOverMenu: PlayerController not found.");
        }

        // Reset enemy spawner
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.ResetSpawner();
            Debug.Log("GameOverMenu: EnemySpawner reset.");
        }
        else
        {
            Debug.LogWarning("GameOverMenu: EnemySpawner not found.");
        }
    }

    /// <summary>
    /// Transitions from the Game Over screen back to the Main Menu.
    /// </summary>
    public void MainMenu()
    {
        // Ensure game time is normal before transitioning to menu
        Time.timeScale = 1f;
        GameOver = false;

        // Hide the game over UI
        GameOverUI.SetActive(false);
        GameManager.Instance.ResetGame();

        SceneManager.LoadScene("MainScene");
    }
}
