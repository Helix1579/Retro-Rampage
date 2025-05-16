using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public int scoreValue = 10; // Can vary by enemy type

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} (enemy) died!");

        GameManager.Instance?.AddScore(scoreValue);

        Destroy(gameObject);
    }
}
