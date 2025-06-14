using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IHealth
{
    public int maxHealth = 5;
    protected int currentHealth;
    
    public int CurrentHealth => currentHealth;

    public System.Action<int> OnHealthChanged;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth); // <- notify healthbar

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth); // <- notify healthbar
    }

    protected abstract void Die();
    
    public virtual void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); // Update healthbar if bound
    }
}