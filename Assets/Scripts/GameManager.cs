using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerScore = 0;

    [Header("Weapon Unlocks")]
    public WeaponData weaponAt500;
    public WeaponData weaponAt1500;

    [Header("Difficulty")]
public string selectedDifficulty = "Normal";


public void SetDifficulty(string difficulty)
{
    selectedDifficulty = difficulty;
    Debug.Log("Difficulty selected: " + difficulty);
}
    private void Awake()
    {
        // Setup Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist between scenes
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
}
