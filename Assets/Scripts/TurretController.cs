using UnityEngine;

public class TurretController : MonoBehaviour
{
    private float fireRate;
    private float fireCooldown;
    private EnemyShooter shooter;
    private Transform target;
    
    public Transform Target
    {
        get => target;
        set => target = value;
    }

    void Awake()
    {
        shooter = GetComponent<EnemyShooter>();
        target = shooter.player;
        shooter.fireRate = fireRate; // Sync rate with EnemyShooter's fireRate
    }

    void Update()
    {
        if (target == null || shooter == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > shooter.attackRange) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            fireCooldown = fireRate;

            Vector2 direction = (target.position - shooter.firePoint.position).normalized;
            shooter.Shoot(direction);
        }
    }

    public void UpdateFireRate(float newRate)
    {
        fireRate = newRate;
        if (shooter != null)
        {
            shooter.fireRate = newRate;
        }
    }
}