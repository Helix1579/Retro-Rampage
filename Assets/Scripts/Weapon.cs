using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;  // Visual effect only
    public float bulletSpeed = 20f;  // For visuals, or can be ignored
    public float fireRate = 0.2f;
    public int damage = 1;
    public float bulletLifeTime = 2f;
    public float hitDetectionRadius = 0.5f;  // Radius to detect hits on shooting

    protected float fireCooldown;

    protected virtual void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    public virtual void TryShoot(Vector2 direction, string shooterTag)
    {
        if (fireCooldown > 0f) return;

        fireCooldown = fireRate;

        // Spawn visual bullet prefab (optional, no logic)
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Optional: move the bullet visually if it has Rigidbody2D or Animator
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * bulletSpeed;
            }

            Destroy(bullet, bulletLifeTime);
        }

        // Immediately check for targets in front of firePoint in the shooting direction
        RaycastHit2D hit = Physics2D.CircleCast(firePoint.position, hitDetectionRadius, direction, 10f);
        if (hit.collider != null)
        {
            // Only damage enemies if player shoots, and player if enemy shoots
            if ((shooterTag == "Player" && hit.collider.CompareTag("Enemy")) ||
                (shooterTag == "Enemy" && hit.collider.CompareTag("Player")))
            {
                IHealth health = hit.collider.GetComponent<IHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }
}
