using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;
    public int damage = 1;
    public float bulletLifeTime = 2f;
    public AudioClip fireSound;
    public AudioSource audioSource;
    
    protected float fireCooldown;
    
    protected EnemyConfig currentConfig;

    protected virtual void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    public void TryShoot(Vector2 direction, string shooterTag)
    {
        if (fireCooldown > 0f || bulletPrefab == null || firePoint == null)
            return;

        fireCooldown = fireRate;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (bulletObj.TryGetComponent(out Bullet bullet))
        {
            float speed = currentConfig != null ? bulletSpeed : bulletSpeed;
            int bulletDamage = currentConfig != null ? currentConfig.damage : damage;

            bullet.SetDirection(direction, speed, shooterTag);
            bullet.damage = bulletDamage;
        }
        
        if (fireSound != null)
        {
            // Get or cache the AudioSource
            audioSource.PlayOneShot(fireSound);
        }

        Destroy(bulletObj, bulletLifeTime);
    }
}
