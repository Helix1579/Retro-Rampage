using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyFactory enemyFactory;
    public EnemyConfig enemyConfig;
    public Transform player;

    public int maxEnemiesToSpawn = 50;
    public int maxEnemiesAliveAtOnce = 10;

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
        if (enemiesSpawned >= maxEnemiesToSpawn) return;
        if (enemiesAlive >= maxEnemiesAliveAtOnce) return;

        if (player.position.x - lastSpawnX >= 2f)
        {
            SpawnRandomEnemyNearPlayer();
            lastSpawnX = player.position.x;
        }
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

        GameObject enemy = enemyFactory.SpawnEnemy(randomType, spawnPos, enemyConfig);

        if (enemy != null)
        {
            enemiesSpawned++;
            enemiesAlive++;

            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
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
}
