using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 15f;
    public float fireRate = 1f;
    public float attackRange = 6f;

    private float fireCooldown;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        fireCooldown -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && fireCooldown <= 0f)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            Shoot(direction);
            fireCooldown = fireRate;
        }
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
       bulletScript.SetDirection(direction, bulletSpeed, "Enemy");
    }
}
