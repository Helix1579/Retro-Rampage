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
      
        GameOverUI.SetActive(false);
        Time.timeScale = 1f;
        GameOver = false;
    }
    
    public void ShowGameOver()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        GameOver = true;

        // Display the player's score if GameManager and scoreText are available
        if (GameManager.Instance != null && scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.playerScore}";
        }

        StartCoroutine(SelectFirstButton());
    }

    private IEnumerator SelectFirstButton()
    {
        yield return null; // Wait one frame for UI to initialize
        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        }
        else
        {
            Debug.LogWarning("GameOverMenu: EventSystem or firstSelectedButton is null. Cannot select first button.");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameOver = false;
        GameOverUI.SetActive(false);
        GameObject gameplayGroup = GameObject.Find("GamePlay");
        PlayerController player = FindObjectOfType<PlayerController>();
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();

        // Reset game state via GameManager
        if (GameManager.Instance != null && gameplayGroup != null && player != null && spawner != null)
        {
            GameManager.Instance.ResetGame();
            gameplayGroup.SetActive(true);
            player.ResetPlayer();
            spawner.ResetSpawner();
        }
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameOver = false;

        GameOverUI.SetActive(false);
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene("MainScene");
    }
}
