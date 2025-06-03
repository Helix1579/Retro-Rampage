using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerScore = 0;

    [Header("Weapon Unlocks")]
    public WeaponData weaponAt400;
    public WeaponData weaponAt750;

    private WeaponData defaultWeaponAt400;
    private WeaponData defaultWeaponAt750;

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
            Instance = this;
            DontDestroyOnLoad(gameObject);

            defaultWeaponAt400 = weaponAt400;
            defaultWeaponAt750 = weaponAt750;
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

        if (playerScore >= 500 && weaponAt400 != null)
        {
            shooter.UnlockWeapon(weaponAt400);
            weaponAt400 = null; // Prevent unlocking again
        }

        if (playerScore >= 750 && weaponAt750 != null)
        {
            shooter.UnlockWeapon(weaponAt750);
            weaponAt750 = null;
        }
    }

    public void ResetGame()
    {
        playerScore = 0;
        Debug.Log("Reset game");

        weaponAt400 = defaultWeaponAt400;
        weaponAt750 = defaultWeaponAt750;
    }
}