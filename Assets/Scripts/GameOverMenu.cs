using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;
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

        // ✅ Only update the score now — GameManager should be ready
        if (GameManager.Instance != null && scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.playerScore}";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameOver = false;
        GameOverUI.SetActive(false);

        GameManager.Instance.ResetGame();

        // GameObject mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        // if (mainMenuCanvas != null)
        // {
        //     mainMenuCanvas.SetActive(false);
        // }

        GameObject gameplayGroup = GameObject.Find("GameplayGroup");
        if (gameplayGroup != null)
        {
            gameplayGroup.SetActive(true);
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
}