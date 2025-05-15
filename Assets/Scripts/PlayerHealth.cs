using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died.");
        // Add death animation, respawn, game over etc.
    }
}
