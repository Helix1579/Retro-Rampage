using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject gameplayGroup;

private void Start()
{
    Time.timeScale = 0f;

    difficultyPanel.SetActive(false);
    gameplayGroup.SetActive(false);

    // ✅ Play menu music immediately when main menu appears
    if (MusicManager.Instance != null)
    {
        MusicManager.Instance.PlayMenuMusic();
    }
}


public void OnSelectDifficulty(string difficulty)
{
    GameManager.Instance?.SetDifficulty(difficulty);

    Time.timeScale = 1f;
    gameObject.SetActive(false);         // Hide menu canvas
    gameplayGroup.SetActive(true);       // Show gameplay elements

    // ✅ Switch to gameplay music
    MusicManager.Instance?.PlayGameplayMusic();
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

}
