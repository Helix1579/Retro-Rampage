using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public GameObject patrolEnemyPrefab;
    public GameObject rangedChaseEnemyPrefab;

    public GameObject SpawnEnemy(EnemyType type, Vector3 position, EnemyConfig config)
    {
        GameObject prefab = null;
        switch (type)
        {
            case EnemyType.Patrol:
                prefab = patrolEnemyPrefab;
                break;
            case EnemyType.RangedChase:
                prefab = rangedChaseEnemyPrefab;
                break;
        }

        if (prefab == null)
        {
            return null;
        }

        GameObject enemy = Instantiate(prefab, position, Quaternion.identity);

        var enemyAI = enemy.GetComponent<IEnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.SetTarget(FindObjectOfType<PlayerController>()?.transform);
            enemyAI.Init(config);

            // Find the firePoint transform inside the spawned enemy hierarchy
            Transform firePoint = enemy.transform.Find("FirePoint");
            if (firePoint == null)
            {
            }
            enemyAI.SetFirePoint(firePoint);
        }

        return enemy;
    }
}
