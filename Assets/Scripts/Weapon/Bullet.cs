using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject impactEffect;
    public int damage = 1;
    private Vector2 moveDirection;
    private float moveSpeed;
    private string shooterTag;

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
        // Ignore friendly fire
        if ((shooterTag == "Player" && other.CompareTag("Player")) ||
            (shooterTag == "Enemy" && other.CompareTag("Enemy")))
        {
            return;
        }

        IHealth health = other.GetComponent<IHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
