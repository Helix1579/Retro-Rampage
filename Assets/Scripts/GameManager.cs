using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerScore = 0;

    [Header("Weapon Unlocks")]
    public WeaponData weaponAt500;
    public WeaponData weaponAt1500;

    private WeaponData defaultWeaponAt500;
    private WeaponData defaultWeaponAt1500;

    [Header("Difficulty")]
    public DifficultySetting currentDifficulty;
    public DifficultySetting noobSettings;
    public DifficultySetting proSettings;
    public DifficultySetting rampageSettings;


    public void SetDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Noob":
                currentDifficulty = noobSettings;
                break;
            case "Pro":
                currentDifficulty = proSettings;
                break;
            case "Rampage":
                currentDifficulty = rampageSettings;
                break;
            default:
                Debug.LogWarning("Unknown difficulty selected, using Pro as default.");
                currentDifficulty = proSettings;
                break;
        }

        Debug.Log($"Difficulty selected: {currentDifficulty.level}");
        // Initialize the boss after setting difficulty
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("GameManager instance created.");
            Instance = this;
            DontDestroyOnLoad(gameObject);

            defaultWeaponAt500 = weaponAt500;
            defaultWeaponAt1500 = weaponAt1500;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
        Debug.Log("Score: " + playerScore);

        PlayerShooter shooter = FindObjectOfType<PlayerShooter>();
        if (shooter == null) return;

        if (playerScore >= 500 && weaponAt500 != null)
        {
            shooter.UnlockWeapon(weaponAt500);
            weaponAt500 = null; // Prevent unlocking again
        }

        if (playerScore >= 1500 && weaponAt1500 != null)
        {
            shooter.UnlockWeapon(weaponAt1500);
            weaponAt1500 = null;
        }
    }

    public void ResetGame()
    {
        playerScore = 0;
        Debug.Log("Reset game");

        weaponAt500 = defaultWeaponAt500;
        weaponAt1500 = defaultWeaponAt1500;

        Debug.Log("GameManager reset.");
    }
}