using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerWinMenu : MonoBehaviour
{
    public GameObject PlayerWinUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    private static bool GameOver = false;

    void Awake()
    {
        PlayerWinUI.SetActive(false);
        Time.timeScale = 1f;
        GameOver = false;
    }

    public void ShowPlayerWin()
    {
        PlayerWinUI.SetActive(true);
        Time.timeScale = 0f;
        GameOver = true;

        // ✅ Only update the score now — GameManager should be ready
        if (GameManager.Instance != null && scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.playerScore}";
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameOver = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        GameManager.Instance.ResetGame();

        // ✅ Load the main menu scene
        SceneManager.LoadScene("MainScene"); // Make sure this name matches your scene exactly
    }

}
