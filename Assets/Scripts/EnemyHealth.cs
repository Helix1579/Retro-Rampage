using UnityEngine;

public class EnemyHealth : BaseHealth
{
    protected override void Die()
    {
        Debug.Log($"{gameObject.name} (enemy) died!");
        // Play death animation, drop loot, etc.
        Destroy(gameObject);
    }
}
