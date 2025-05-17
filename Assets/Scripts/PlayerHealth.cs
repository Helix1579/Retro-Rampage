using UnityEngine;

public class PlayerHealth : BaseHealth
{
    protected override void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
    }
}
