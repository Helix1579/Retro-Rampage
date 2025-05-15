using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    // private int currentHealth;

    public GameObject deathEffect;
    
    public void TakeDamage(int damage)
    {
        maxHealth -= damage;

        if (maxHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}