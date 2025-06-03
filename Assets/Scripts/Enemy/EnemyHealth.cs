using UnityEngine;
using System;

public class EnemyHealth : BaseHealth
{
    public int scoreValue = 10;
    public event Action OnDeathEvent;

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} (enemy) died!");

        GameManager.Instance?.AddScore(scoreValue);

        OnDeathEvent?.Invoke();

        Destroy(gameObject);
    }
    public void SetHealth(int amount)
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

}
