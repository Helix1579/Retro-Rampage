using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyFactory enemyFactory;
    public EnemyConfig enemyConfig;
    public Transform player;

   private DifficultySetting Difficulty => GameManager.Instance.currentDifficulty;


    public float spawnDistanceAhead = 15f;
    public float spawnDistanceBehind = 5f;
    public float spawnHeightMin = -1f;
    public float spawnHeightMax = 3f;

    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;
    private float lastSpawnX = 0f;

    void Update()
    {
        if (player == null) return;
       if (enemiesSpawned >= Difficulty.maxEnemiesToSpawn) return;
        if (enemiesAlive >= Difficulty.maxEnemiesAliveAtOnce) return;

        if (player.position.x - lastSpawnX >= 2f)
        {
            SpawnRandomEnemyNearPlayer();
            lastSpawnX = player.position.x;
        }

        Debug.Log("Spawner Update running");
    }

    void SpawnRandomEnemyNearPlayer()
    {
        float spawnX = Random.Range(player.position.x - spawnDistanceBehind, player.position.x + spawnDistanceAhead);
        Vector3 spawnPos;
        EnemyType randomType = GetRandomEnemyType();

        if (randomType == EnemyType.Turret)
        {
            // Use Physics2D.Raycast for 2D games
            Vector2 rayOrigin = new Vector2(spawnX, 10f); // start above
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f);

            if (hit.collider != null)
            {
                spawnPos = hit.point + Vector2.up * 0.1f;
                Debug.Log($"✅ Turret spawned at ground point: {spawnPos}");
            }
            else
            {
                spawnPos = new Vector3(spawnX, 0f, 0f); // fallback
                Debug.LogWarning("⚠️ Turret spawn: ground not found, defaulting to y = 0.");
            }
        }
        else
        {
            float spawnY = Random.Range(spawnHeightMin, spawnHeightMax);
            spawnPos = new Vector3(spawnX, spawnY, 0f);
        }

        // Create a modified config based on difficulty
EnemyConfig config = new EnemyConfig
{
    fireDistance = enemyConfig.fireDistance,
    pointA = enemyConfig.pointA,
    pointB = enemyConfig.pointB,

    // 🔥 Override with difficulty-based damage
    damage = GameManager.Instance.currentDifficulty.enemyDamage
};
GameObject enemy = enemyFactory.SpawnEnemy(randomType, spawnPos, config);


        if (enemy != null)
        {
            enemiesSpawned++;
            enemiesAlive++;

            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.SetHealth(GameManager.Instance.currentDifficulty.enemyHealth);
                enemyHealth.OnDeathEvent += () => enemiesAlive--;
            }
        }
    }

    EnemyType GetRandomEnemyType()
    {
        int rand = Random.Range(0, 3); // 0 = Patrol, 1 = RangedChase, 2 = Turret
        switch (rand)
        {
            case 0: return EnemyType.Patrol;
            case 1: return EnemyType.RangedChase;
            case 2: return EnemyType.Turret;
            default: return EnemyType.Patrol;
        }
    }
    
    public void ResetSpawner()
    {
        enemiesSpawned = 0;
        enemiesAlive = 0;
        lastSpawnX = 0f;

        // Destroy all enemies
        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }

        // Destroy all bullets/projectiles
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        Debug.Log("EnemySpawner has been reset (enemies and bullets cleared).");
    }

}
