using UnityEngine;
using UnityEngine.EventSystems;

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
    void Start()
    {
        // EventSystem.current.SetSelectedGameObject();
    }

}


public void OnSelectDifficulty(string difficulty)
{
    GameManager.Instance?.SetDifficulty(difficulty);

    Time.timeScale = 1f;
    gameObject.SetActive(false);         // Hide menu canvas
    gameplayGroup.SetActive(true);       // Show gameplay elements
    if (GameManager.Instance != null)
        {
            // Find the boss *now* that it's active
            BossAI boss = FindObjectOfType<BossAI>();
            TurretAI[] turrets = FindObjectsOfType<TurretAI>();
foreach (var turret in turrets)
{
    turret.InitializeTurret();
}
            
            if (boss != null)
            {
                boss.InitializeBoss(); // Call the boss's initialization method
                Debug.Log("MainMenuManager: BossAI initialized after gameplay group activated.");
            }
            else
            {
                Debug.LogError("MainMenuManager: BossAI not found after activating gameplay group! Is it part of gameplayGroup or active?");
            }
        }
        else
        {
            Debug.LogError("MainMenuManager: GameManager instance is null!");
        }

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
