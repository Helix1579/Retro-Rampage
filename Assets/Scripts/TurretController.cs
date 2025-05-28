using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float detectionRange = 5f;
    public Transform target;

    private float fireCooldown;

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > detectionRange) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            fireCooldown = fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (bullet.TryGetComponent(out Bullet b))
            b.SetDirection(direction, 10f, "Enemy");

        Destroy(bullet, 3f);
    }

    public void UpdateFireRate(float newRate)
    {
        fireRate = newRate;
    }
}