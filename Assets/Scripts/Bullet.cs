using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject impactEffect;
    public int damage = 1;
    private Vector2 moveDirection;
    private float moveSpeed;

    private string shooterTag; // NEW: to track who fired

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction, float speed, string shooterTag = "Player")
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        this.shooterTag = shooterTag;

        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        if (shooterTag == "Player" && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
        else if (shooterTag == "Enemy" && other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
                player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
