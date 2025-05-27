using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemyAI
{
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
        fireCooldown = config.fireRate;
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    void Update()
    {
        if (player == null || firePoint == null ) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= config.detectionRange)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                fireCooldown = config.fireRate;
                ShootAtPlayer();
            }
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
