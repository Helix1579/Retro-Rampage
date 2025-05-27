using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficultySettings", menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public string difficultyName; // e.g., "Noob", "Pro", "Rampage"
    public int maxEnemiesToSpawn;
    public int maxEnemiesAliveAtOnce;
    public float enemyDamageMultiplier; // Multiplier for enemy damage (e.g., 0.5 for Noob, 1.0 for Pro, 1.5 for Rampage)
    public float enemyHealthMultiplier; // Multiplier for enemy health (e.g., 0.7 for Noob, 1.0 for Pro, 1.3 for Rampage)
}