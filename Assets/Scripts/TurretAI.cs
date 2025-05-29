using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemyAI
{
    private Transform player;
    private Transform firePoint;
    private float fireCooldown;
    private EnemyShooter shooter;
    private EnemyHealth turretHealth;
    private bool isInitialized = false;

    private EnemyConfig config;

void Awake()
    {
        turretHealth = GetComponent<EnemyHealth>();
        if (turretHealth == null)
        {
            Debug.LogError("TurretAI: EnemyHealth component not found on turret!", this);
        }
    }
    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void Init(EnemyConfig cfg)
    {
        config = cfg;
        shooter = GetComponent<EnemyShooter>();
        fireCooldown = shooter.fireRate;
    }

    public void SetFirePoint(Transform fp)
    {
        firePoint = fp;
    }

    public void InitializeTurret()
{
    if (isInitialized)
    {
        Debug.LogWarning("TurretAI: Already initialized.");
        return;
    }

    if (turretHealth == null)
    {
        Debug.LogError("TurretAI: Cannot initialize because EnemyHealth is missing.");
        return;
    }

    int startHealth = 50;

    if (GameManager.Instance != null && GameManager.Instance.currentDifficulty != null)
    {
        startHealth = GameManager.Instance.currentDifficulty.turretHealth;
        Debug.Log($"TurretAI: Initialized with {startHealth} HP from GameManager.");
    }
    else
    {
        Debug.LogWarning("TurretAI: GameManager or difficulty is null. Using default health.");
    }

    turretHealth.SetHealth(startHealth);
    isInitialized = true;
}



    void Update()
    {
        if (player == null || firePoint == null || shooter == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= shooter.attackRange)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                fireCooldown = shooter.fireRate;

                Vector2 direction = (player.position - firePoint.position).normalized;
                shooter.Shoot(direction);
            }
        }
    }
}