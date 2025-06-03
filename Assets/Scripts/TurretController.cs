using UnityEngine;

public class TurretController : MonoBehaviour
{
    private float fireRate;
    private float range;
    private float fireCooldown;
    private EnemyShooter shooter;
    private Transform target;
    private GameObject bulletPrefab;
    private SpriteRenderer turretSprite;
    private AudioClip fireSound;
    
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
        shooter.attackRange = range;
        shooter.bulletPrefab = bulletPrefab;
        turretSprite = GetComponent<SpriteRenderer>();
        shooter.fireSound = fireSound;
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
    public void UpdateAttackRange(float newRange)
    {
        range = newRange;
        if (shooter != null)
        {
            shooter.attackRange = newRange;
        }
    }
    
    public void UpdateBulletPrefab(GameObject newPrefab)
    {
        bulletPrefab = newPrefab;
        if (shooter != null)
        {
            shooter.bulletPrefab = newPrefab;
        }
    }
    
    public void UpdateTurretSprite(Sprite newSprite)
    {
        if (turretSprite != null && newSprite != null)
        {
            turretSprite.sprite = newSprite;
        }
    }
    public void UpdateTurretSound(AudioClip newSound)
    {
        fireSound = newSound;
        if (fireSound != null)
        {
            shooter.fireSound = newSound;
        }
    }
}