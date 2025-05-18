using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public HealthBar healthBar;

    protected override void Start()
    {
        base.Start();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);

            OnHealthChanged += healthBar.SetHealth;
        }
    }
    



    protected override void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
    }
}