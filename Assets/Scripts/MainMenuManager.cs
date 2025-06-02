

using UnityEngine;
using UnityEngine.EventSystems; // Required for EventSystem
using UnityEngine.UI; // Required for Button component

public class MainMenuManager : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject gameplayGroup;
    public GameObject playButton; // Assign your Play Button GameObject here in the Inspector
    public Button firstDifficultyButton; // Assign the first difficulty button (e.g., Easy) here

    private void Start()
    {
        // Pause the game initially
        Time.timeScale = 0f;

        // Hide difficulty and gameplay elements
        difficultyPanel.SetActive(false);
        gameplayGroup.SetActive(false);

        // ✅ Play menu music immediately when main menu appears
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMenuMusic();
        }

        // --- IMPORTANT: Set the initially selected UI element for gamepad navigation ---
        // Ensure the EventSystem exists in your scene (usually added automatically with a Canvas).
        // Set the 'playButton' as the currently selected GameObject.
        if (EventSystem.current != null && playButton != null)
        {
            EventSystem.current.SetSelectedGameObject(playButton);
            Debug.Log("MainMenuManager: Play Button set as initial selected GameObject for EventSystem.");
        }
        else
        {
            if (EventSystem.current == null)
            {
                Debug.LogError("MainMenuManager: EventSystem not found in the scene! Please ensure you have an EventSystem GameObject.");
            }
            if (playButton == null)
            {
                Debug.LogError("MainMenuManager: Play Button GameObject is not assigned in the Inspector!");
            }
        }
    }


    public void OnSelectDifficulty(string difficulty)
    {
        GameManager.Instance?.SetDifficulty(difficulty);

        Time.timeScale = 1f;
        gameObject.SetActive(false);         // Hide menu canvas
        gameplayGroup.SetActive(true);       // Show gameplay elements

        // Clear selected UI element to prevent accidental input after game starts
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("MainMenuManager: Cleared selected GameObject as gameplay starts.");
        }

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
        if (playButton != null)
            playButton.SetActive(false);

        // --- IMPORTANT: Set the initially selected UI element for the difficulty panel ---
        // After the difficulty panel is shown, set the first difficulty button as selected.
        if (EventSystem.current != null && firstDifficultyButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstDifficultyButton.gameObject);
            Debug.Log("MainMenuManager: First Difficulty Button set as initial selected GameObject for EventSystem.");
        }
        else
        {
            if (EventSystem.current == null)
            {
                Debug.LogError("MainMenuManager: EventSystem not found when trying to select difficulty button!");
            }
            if (firstDifficultyButton == null)
            {
                Debug.LogError("MainMenuManager: First Difficulty Button is not assigned in the Inspector!");
            }
        }
    }
}
