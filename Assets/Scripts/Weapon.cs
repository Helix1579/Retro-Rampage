using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;
    public int damage = 1;
    public float bulletLifeTime = 2f;
    protected float fireCooldown;
    private EnemyConfig currentConfig;

    protected virtual void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    public virtual void TryShoot(Vector2 direction, string shooterTag)
    {
        if (fireCooldown > 0f || bulletPrefab == null || firePoint == null)
            return;

        fireCooldown = fireRate;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Optional: Rotate the bullet to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (bulletObj.TryGetComponent(out Bullet bullet))
        {
            float speed = currentConfig != null ? bulletSpeed : bulletSpeed; // Replace this with currentConfig.speed if needed
            int bulletDamage = currentConfig != null ? currentConfig.damage : damage;

            bullet.SetDirection(direction, speed, shooterTag);
            bullet.damage = bulletDamage;
        }

        Destroy(bulletObj, bulletLifeTime);
    }

}
