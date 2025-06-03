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
        Debug.Log("ShowPlayerWin called");
        PlayerWinUI.SetActive(true);

        Time.timeScale = 0f;
        GameOver = true;
        scoreText.text = $"Score: {GameManager.Instance.playerScore}";

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        PlayerWinUI.SetActive(false);
        GameOver = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        GameManager.Instance.ResetGame();
        
        SceneManager.LoadScene("MainScene");
    }

}
