using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemyAI
{
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;
    private float fireCooldown;
    private EnemyShooter shooter;

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        shooter = GetComponent<EnemyShooter>();
        fireCooldown = shooter.fireRate;
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    void Update()
    {
        if (player == null || firePoint == null || shooter == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= shooter.attackRange)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                fireCooldown = shooter.fireRate;

                Vector2 direction = (player.position - firePoint.position).normalized;
                shooter.Shoot(direction);
            }
        }
    }
}