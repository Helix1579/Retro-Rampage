using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossPhase { Phase1, Phase2, Phase3, Dead }
    private BossPhase currentPhase = BossPhase.Phase1;

    [Header("Combat Settings")]
    public Transform player;
    public float[] phaseFireRates = { 2f, 1.2f, 0.6f };
    public float[] phaseAttackRange = { 10f, 8f, 5f };
    [Header("Bullet Prefabs Per Phase")]
    public GameObject[] bulletPrefabs;
    public Sprite[] turretSprites;

    [Header("Turrets")]
    public TurretController[] turrets;

    [Header("Phase Thresholds")]
    public int phase2Threshold = 70;
    public int phase3Threshold = 35;
    
    private int bossHp;
    
    
    private EnemyHealth bossHealth;
        private bool isInitialized = false;

   void Awake()
    {
        bossHealth = GetComponent<EnemyHealth>();
        if (bossHealth == null)
        {
            Debug.LogError("BossAI: EnemyHealth component not found on this GameObject!");
            enabled = false; // Disable the script if it can't function
            return;
        }

        // <<< IMPORTANT CHANGE: DO NOT set health here. It's too early.
        bossHealth.OnDeathEvent += OnBossDeath;

        // Turret target can be set here as it doesn't depend on difficulty
        foreach (var turret in turrets)
            turret.Target = player;

        // Debug.Log($"BossAI: Awake completed. Awaiting full initialization."); // Optional: for debugging
    }

    // <<< NEW METHOD: Call this after difficulty is set
    public void InitializeBoss()
    {
        if (isInitialized)
        {
            Debug.LogWarning("BossAI: Attempted to initialize boss multiple times.");
            return; // Prevent re-initialization
        }

        int bossStartHealth = 100; // Default fallback if GameManager is not ready
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.currentDifficulty != null)
            {
                bossStartHealth = GameManager.Instance.currentDifficulty.bossHealth;
                Debug.Log($"BossAI: Initializing with {bossStartHealth} HP from GameManager.");
            }
            else
            {
                Debug.LogWarning("BossAI: GameManager.Instance.currentDifficulty is null during InitializeBoss. Using default 100 HP.");
            }
        }
        else
        {
            Debug.LogWarning("BossAI: GameManager.Instance is null during InitializeBoss. Using default 100 HP.");
        }

        bossHealth.SetHealth(bossStartHealth); // <<< Health is set here!
        bossHp = bossHealth.CurrentHealth; // Update the internal tracking variable

        // Set initial turret fire rate, prefab, etc.
        SetTurretsFireRate(phaseFireRates[0], bulletPrefabs[0], phaseAttackRange[0], turretSprites[0]);
        Debug.Log($"BossAI: Fully initialized with {bossHp} HP, starting phase: {currentPhase}");
        
        isInitialized = true; // Mark as initialized
    }

    void Update()
    {
        // <<< IMPORTANT CHANGE: Only run Update logic if initialized
        if (!isInitialized || currentPhase == BossPhase.Dead || player == null) return;

        int currentHp = bossHealth.CurrentHealth;

        if (currentHp != bossHp)
        {
            HandlePhaseTransition(currentHp);
            bossHp = currentHp;
        }
    }


    void HandlePhaseTransition(int hp)
    {
        if (hp <= 0)
        {
            EnterPhase(BossPhase.Dead);
            return;
        }

        if (hp <= phase3Threshold && currentPhase != BossPhase.Phase3)
        {
            EnterPhase(BossPhase.Phase3);
        }
        else if (hp <= phase2Threshold && currentPhase != BossPhase.Phase2)
        {
            EnterPhase(BossPhase.Phase2);
        }
        else if (hp > phase2Threshold && currentPhase != BossPhase.Phase1)
        {
            EnterPhase(BossPhase.Phase1);
        }
    }


    void EnterPhase(BossPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        Debug.Log($"Boss entered {(int)newPhase}, bullet prefab: {bulletPrefabs[(int)newPhase]?.name}");

        if (newPhase == BossPhase.Dead)
        {
            SetTurretsFireRate(0f, null, 0f, null);
            return;
        }

        SetTurretsFireRate(phaseFireRates[(int)newPhase], bulletPrefabs[(int)newPhase], phaseAttackRange[(int)newPhase], turretSprites[(int)newPhase]);
    }

    void SetTurretsFireRate(float rate, GameObject prefab, float range, Sprite sprite)
    {
        foreach (var turret in turrets)
        {
            turret.UpdateFireRate(rate);
            turret.UpdateBulletPrefab(prefab);
            turret.UpdateAttackRange(range);
            turret.UpdateTurretSprite(sprite);
        }
    }
    void OnBossDeath()
    {
        currentPhase = BossPhase.Dead;
        Debug.Log("Boss defeated!");
    }
}