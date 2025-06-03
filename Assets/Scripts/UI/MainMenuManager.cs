

using UnityEngine;
using UnityEngine.EventSystems; // Required for EventSystem
using UnityEngine.UI; // Required for Button component

public class MainMenuManager : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject gameplayGroup;
    public GameObject playButton; 
    public Button firstDifficultyButton;

    private void Start()
    {
        Time.timeScale = 0f;
        
        difficultyPanel.SetActive(false);
        gameplayGroup.SetActive(false);
        
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMenuMusic();
        }
        
        if (EventSystem.current != null && playButton != null)
        {
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }


    public void OnSelectDifficulty(string difficulty)
    {
        GameManager.Instance?.SetDifficulty(difficulty);

        Time.timeScale = 1f;
        gameObject.SetActive(false);
        gameplayGroup.SetActive(true);
        
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("MainMenuManager: Cleared selected GameObject as gameplay starts.");
        }

        if (GameManager.Instance != null)
        {
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
        }
        MusicManager.Instance?.PlayGameplayMusic();
    }
    
    public void OnPlayPressed()
    {
        difficultyPanel.SetActive(true);

        if (playButton != null)
            playButton.SetActive(false);

        if (EventSystem.current != null && firstDifficultyButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstDifficultyButton.gameObject);
            Debug.Log("MainMenuManager: First Difficulty Button set as initial selected GameObject for EventSystem.");
        }
    }
}
