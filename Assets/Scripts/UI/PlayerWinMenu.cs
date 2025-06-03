using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerWinMenu : MonoBehaviour
{
    public GameObject PlayerWinUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button firstSelectedButton;
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
        StartCoroutine(SelectFirstButton());
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

}
