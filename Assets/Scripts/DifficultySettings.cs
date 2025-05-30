[System.Serializable]
public class DifficultySetting
{
    public DifficultyLevel level;

    public int maxEnemiesToSpawn;
    public int maxEnemiesAliveAtOnce;

    // Damage values used across all types
    public int enemyDamage;

    // Health per enemy type
    public int turretHealth;
    public int patrolHealth;
    public int rangedChaseHealth;
    public int bossHealth;
}
