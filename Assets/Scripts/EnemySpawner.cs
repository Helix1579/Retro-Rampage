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
    float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
    float cameraEdgeX = player.position.x + cameraWidth / 2f;

    float spawnX = Random.Range(cameraEdgeX + 2f, cameraEdgeX + spawnDistanceAhead);

    Vector3 spawnPos;
    EnemyType randomType = GetRandomEnemyType();

    // Raycast to find ground
    Vector2 rayOrigin = new Vector2(spawnX, 10f);
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f);

    if (hit.collider != null)
    {
        spawnPos = hit.point + Vector2.up * 0.1f;
        Debug.Log($"✅ {randomType} spawned at ground point: {spawnPos}");
    }
    else
    {
        Debug.LogWarning($"⚠️ {randomType} spawn: ground not found at {spawnX}, defaulting to y = 0.");
        spawnPos = new Vector3(spawnX, 0f, 0f);
    }

    // Set up config based on difficulty
    EnemyConfig config = new EnemyConfig
    {
        fireDistance = enemyConfig.fireDistance,
        pointA = enemyConfig.pointA,
        pointB = enemyConfig.pointB,
        damage = Difficulty.enemyDamage
    };

    GameObject enemy = enemyFactory.SpawnEnemy(randomType, spawnPos, config);

    if (enemy != null)
    {
        enemiesSpawned++;
        enemiesAlive++;

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            int healthToAssign = 1; // fallback

            if (enemy.GetComponent<TurretAI>() != null)
                healthToAssign = Difficulty.turretHealth;
            else if (enemy.GetComponent<PatrolAI>() != null)
                healthToAssign = Difficulty.patrolHealth;
            else if (enemy.GetComponent<RangedChaseAI>() != null)
                healthToAssign = Difficulty.rangedChaseHealth;

            enemyHealth.SetHealth(healthToAssign);
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
