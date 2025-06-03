using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour, IEnemyAI
{
    private NavMeshAgent agent;
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;
    private bool goingToA = true;
    private bool isProvoked = false;
    private float fireCooldown;
    private EnemyShooter enemy;
    private bool isFacingRight = true;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        enemy = GetComponent<EnemyShooter>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = 2f;
        }

        if (enemy != null)
        {
            enemy.SetConfig(cfg);
        }
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    void Update()
    {
        if (agent == null || config.pointA == null || config.pointB == null) return;

        if (player == null || enemy == null || firePoint == null)
        {
            Patrol();
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        // Detect player
        if (dist <= enemy.attackRange)
        {
            isProvoked = true;
        }
        else if (dist > enemy.attackRange + 1f)
        {
            isProvoked = false;
        }

        if (isProvoked)
        {
            FlipTowardsPlayer();
            agent.SetDestination(player.position);

            if (dist <= config.fireDistance)
            {
                agent.ResetPath();

                fireCooldown -= Time.deltaTime;
                if (fireCooldown <= 0f)
                {
                    fireCooldown = enemy.fireRate;
                    Vector2 direction = (player.position - firePoint.position).normalized;
                    enemy.Shoot(direction);
                }
            }
        }
        else
        {
            Patrol();
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

    void FlipTowardsPlayer()
    {
        if (player == null || firePoint == null) return;

        bool shouldFaceRight = player.position.x > transform.position.x;

        if (shouldFaceRight != isFacingRight)
        {
            isFacingRight = shouldFaceRight;

            // Flip enemy's local X scale
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * (isFacingRight ? 1 : -1);
            transform.localScale = localScale;

            // Flip firePoint's local scale to match facing direction
            Vector3 fpScale = firePoint.localScale;
            fpScale.x = Mathf.Abs(fpScale.x) * (isFacingRight ? 1 : -1);
            firePoint.localScale = fpScale;
        }
    }
}
