using UnityEngine;

public class PlayerHealth : BaseHealth
{
    protected override void Die()
    {
        Debug.Log("Player died!");
        // Disable movement, trigger respawn or game over screen
        gameObject.SetActive(false);
    }
}
