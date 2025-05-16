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

    protected virtual void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    public virtual void TryShoot(Vector2 direction, string shooterTag)
    {
        if (fireCooldown > 0f || bulletPrefab == null)
            return;

        fireCooldown = fireRate;

        // Spawn and initialize the bullet
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(direction, bulletSpeed, shooterTag);
            bullet.damage = damage;
        }

        Destroy(bulletObj, bulletLifeTime);
    }
}
