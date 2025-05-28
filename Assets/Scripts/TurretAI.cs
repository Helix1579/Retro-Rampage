using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemyAI
{
    private Transform player;
    private EnemyConfig config;
    private Transform firePoint;
    private float fireCooldown;

    private Enemy enemy; // ✅ cache prefab data

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        enemy = GetComponent<Enemy>(); // ✅ store prefab reference
        fireCooldown = enemy.fireRate;
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    void Update()
    {
        if (player == null || firePoint == null || enemy == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= enemy.detectionRange)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                fireCooldown = enemy.fireRate;
                ShootAtPlayer();
            }
        }
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