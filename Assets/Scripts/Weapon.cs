using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.1f;

    private PlayerController playerController;
    private float fireCooldown;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            Vector2 shootDirection = GetShootDirection();
            Shoot(shootDirection);
            fireCooldown = fireRate;
        }
    }

    Vector2 GetShootDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            return playerController != null && playerController.IsFacingRight ? Vector2.right : Vector2.left;
        }

        return new Vector2(horizontal, vertical).normalized;
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(direction, bulletSpeed);
    }
}