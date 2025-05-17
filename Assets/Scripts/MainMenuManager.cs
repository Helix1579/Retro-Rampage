using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject gameplayGroup;

    private void Start()
    {
        // Pause game time and show only main menu
        Time.timeScale = 0f;
        difficultyPanel.SetActive(false);
        gameplayGroup.SetActive(false);
    }

    public void OnPlayPressed()
    {
        // Show difficulty options
        difficultyPanel.SetActive(true);

        // Hide play button (optional if needed)
        GameObject playButton = GameObject.Find("PlayButton");
        if (playButton != null)
            playButton.SetActive(false);
    }

    public void OnSelectDifficulty(string difficulty)
    {
        GameManager.Instance?.SetDifficulty(difficulty);

        // Resume time
        Time.timeScale = 1f;

        // Hide entire menu (this script is on MainMenuCanvas)
        gameObject.SetActive(false); // hides Play button + Difficulty panel

        // Enable gameplay
        gameplayGroup.SetActive(true);
    }
}
