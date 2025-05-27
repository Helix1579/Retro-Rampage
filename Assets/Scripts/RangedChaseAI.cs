using UnityEngine;
using UnityEngine.AI;

public class RangedChaseAI : MonoBehaviour, IEnemyAI
{
    private NavMeshAgent agent;
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;
    private float fireCooldown;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
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
        if (player == null || agent == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= config.detectionRange)
        {
            if (distance > config.fireDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath();
                fireCooldown -= Time.deltaTime;

                if (fireCooldown <= 0f  && firePoint != null)
                {
                    fireCooldown = config.fireRate;
                    ShootAtPlayer();
                }
            }
        }
        else
        {
            agent.ResetPath();
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
