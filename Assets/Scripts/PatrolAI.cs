using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour, IEnemyAI
{
    private NavMeshAgent agent;
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;  // now instance specific
    private bool goingToA = true;
    private bool isProvoked = false;
    private float fireCooldown;
    
    private Enemy enemy;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = 2f;
        }
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    void Update()
    {
        if (agent == null || config.pointA == null || config.pointB == null) return;

        Patrol();

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= enemy.detectionRange)
        {
            isProvoked = true;
        }

        if (isProvoked)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f && firePoint != null)
            {
                fireCooldown = GetComponent<Enemy>().fireRate;

                ShootAtPlayer();
            }
        }
    }

    void Patrol()
    {
        Transform target = goingToA ? config.pointA : config.pointB;
        agent.SetDestination(target.position);

        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            goingToA = !goingToA;
        }
    }

    void ShootAtPlayer()
    {
        GameObject bulletPrefab = GetComponent<Enemy>()?.bulletPrefab;
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
