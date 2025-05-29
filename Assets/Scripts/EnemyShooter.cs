using UnityEngine;

public class EnemyShooter : Weapon, IShooter
{
    public Transform player;
    public float attackRange = 6f;
    private EnemyConfig currentConfig;
    public void SetConfig(EnemyConfig config) => currentConfig = config;

    private void Start()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            TryShoot(direction, "Enemy");
        }
    }

    public void Shoot(Vector2 direction)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Rotate bullet to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Apply bullet behavior
        if (bullet.TryGetComponent(out Bullet b))
        {
            b.SetDirection(direction, 10f, "Enemy");
            b.damage = currentConfig?.damage ?? 1;
        }

        Destroy(bullet, 3f);
    }

}
