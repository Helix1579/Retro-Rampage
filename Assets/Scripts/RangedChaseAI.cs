using UnityEngine;
using UnityEngine.AI;

public class RangedChaseAI : MonoBehaviour, IEnemyAI
{
    private NavMeshAgent agent;
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;
    private float fireCooldown;

    private EnemyShooter enemy;
    private bool isProvoked = false;

    private bool isFacingRight = true;
    private Vector3 firePointOriginalLocalPos;
    private Collider2D myCollider;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        enemy = GetComponent<EnemyShooter>();
        agent = GetComponent<NavMeshAgent>();
        
        if (enemy != null)
        {
            enemy.SetConfig(cfg);
        }

        if (agent)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        
        // IgnoreCollisionWithOtherEnemies();
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
        firePointOriginalLocalPos = firePoint.localPosition; // Cache the original position
        Debug.Log("Assigned FirePoint at localPosition.x = " + firePoint.localPosition.x);
    }

    void Update()
    {
        if (player == null || agent == null || enemy == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Engage if within detection range
        if (distance <= enemy.attackRange)
        {
            isProvoked = true;
            Debug.Log("Provoked");
        }

        // Disengage if outside detection range + buffer
        if (distance > enemy.attackRange + 1f)
        {
            isProvoked = false;
            agent.ResetPath();
            Debug.Log("Not Provoked");
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
            FlipTowardsPlayer();

            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                fireCooldown = enemy.fireRate;

                Vector2 direction = (player.position - firePoint.position).normalized;
                enemy.Shoot(direction);
            }

        }
    }

    void FlipTowardsPlayer()
    {
        if (player == null || firePoint == null) return;

        Debug.Log("Enemy scale: " + transform.localScale + ", FirePoint localPos: " + firePoint.localPosition);

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
    
    // public void IgnoreCollisionWithOtherEnemies()
    // {
    //     if (myCollider == null)
    //     {
    //         myCollider = GetComponent<Collider2D>();
    //         if (myCollider == null)
    //         {
    //             Debug.LogWarning("No Collider2D found on enemy for ignoring collisions.");
    //             return;
    //         }
    //     }
    //
    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //     Debug.Log($"Found {enemies.Length} enemies to ignore collisions with.");
    //
    //     foreach (var enemyObj in enemies)
    //     {
    //         if (enemyObj == this.gameObject) continue; // Skip self
    //
    //         Collider2D otherCollider = enemyObj.GetComponent<Collider2D>();
    //         if (otherCollider != null)
    //         {
    //             Debug.Log($"Ignoring collision between {name} and {enemyObj.name}");
    //             Physics2D.IgnoreCollision(myCollider, otherCollider);
    //         }
    //         else
    //         {
    //             Debug.LogWarning($"Enemy {enemyObj.name} has no Collider2D.");
    //         }
    //     }
    // }

}
