using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public GameObject bulletPrefab;
    public GameObject deathEffect;

    public float fireRate = 2f;
    public float detectionRange = 5f;
    public float GetDetectionRange() => detectionRange;

    
    public GameObject GetBulletPrefab() => bulletPrefab;
    
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