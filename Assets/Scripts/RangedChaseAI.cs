using UnityEngine;
using UnityEngine.AI;

public class RangedChaseAI : MonoBehaviour, IEnemyAI
{
    private NavMeshAgent agent;
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;
    private float fireCooldown;
    
    private Enemy enemy;
    private bool isProvoked = false;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();

        if (agent)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    void Update()
    {
        if (player == null || agent == null || enemy == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Engage if within detection range
        if (distance <= enemy.detectionRange)
        {
            isProvoked = true;
            Debug.Log("Provoked");
        }

        // Disengage if outside detection range + buffer
        if (distance > enemy.detectionRange + 1f)
        {
            isProvoked = false;
            agent.ResetPath();
            Debug.Log("not Provoked");
            return;
        }

        if (!isProvoked) return;

        // Chase if out of fire distance but in detection range
        if (distance > config.fireDistance)
        {
            agent.SetDestination(player.position);
            Debug.Log("Chasing player");
        }
        else
        {
            // Stop and shoot
            agent.ResetPath();
            fireCooldown -= Time.deltaTime;

            // 🔐 Extra safety: skip shooting if player left detection range
            if (distance <= enemy.detectionRange && fireCooldown <= 0f && firePoint != null)
            {
                fireCooldown = enemy.fireRate;
                ShootAtPlayer();
            }
        }
        
        Debug.Log($"Distance: {distance}, Provoked: {isProvoked}, Detection: {enemy.detectionRange}, FireDistance: {config.fireDistance}");
    }


    void ShootAtPlayer()
    {
        GameObject bulletPrefab = enemy?.bulletPrefab;
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (bullet.TryGetComponent(out Bullet b))
        {
            b.SetDirection(dir, 10f, "Enemy");
            b.damage = config.damage;
        }

        Destroy(bullet, 3f);
    }
}
