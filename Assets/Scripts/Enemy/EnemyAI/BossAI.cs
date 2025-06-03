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
    
    [Header("Turrets Weapon Sound")]
    public AudioClip[] bulletSounds;

    private int phase2Threshold;
    private int phase3Threshold;
    
    private int bossHp;
    private EnemyHealth bossHealth;
    private bool isInitialized = false;
    [SerializeField] private PlayerWinMenu playerWinMenu;


   void Awake()
    {
        bossHealth = GetComponent<EnemyHealth>();
        if (bossHealth == null)
        {
            enabled = false;
            return;
        }

        bossHealth.OnDeathEvent += OnBossDeath;

        foreach (var turret in turrets)
            turret.Target = player;

    }

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
            bossStartHealth = GameManager.Instance.currentDifficulty.bossHealth;
            phase2Threshold = GameManager.Instance.currentDifficulty.phase2Threshold;
            phase3Threshold = GameManager.Instance.currentDifficulty.phase3Threshold;
            
        }

        bossHealth.SetHealth(bossStartHealth);
        bossHp = bossHealth.CurrentHealth;

        // Set initial turret fire rate, prefab, etc.
        SetTurrets(phaseFireRates[0], bulletPrefabs[0], phaseAttackRange[0], turretSprites[0], bulletSounds[0]);

        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized || currentPhase == BossPhase.Dead || player == null) return;

        int currentHp = Mathf.Clamp(bossHealth.CurrentHealth, 0, bossHp); // prevent negative glitches

        if (currentHp != bossHp)
        {
            Debug.Log($"Boss HP changed: {bossHp} → {currentHp}");
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

        // Always check for the *most advanced* phase first
        if (hp <= phase3Threshold && currentPhase != BossPhase.Phase3)
        {
            EnterPhase(BossPhase.Phase3);
        }
        else if (hp <= phase2Threshold && currentPhase == BossPhase.Phase1)
        {
            EnterPhase(BossPhase.Phase2);
        }
        // No need to go back to Phase1 once you're in Phase2 or 3
    }



    void EnterPhase(BossPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        Debug.Log($"Boss entered {(int)newPhase}, bullet prefab: {bulletPrefabs[(int)newPhase]?.name}");

        if (newPhase == BossPhase.Dead)
        {
            SetTurrets(0f, null, 0f, null, null);
            return;
        }

        SetTurrets(phaseFireRates[(int)newPhase], bulletPrefabs[(int)newPhase], phaseAttackRange[(int)newPhase], turretSprites[(int)newPhase], bulletSounds[(int)newPhase]);
    }

    void SetTurrets(float rate, GameObject prefab, float range, Sprite sprite, AudioClip sound)
    {
        foreach (var turret in turrets)
        {
            turret.UpdateFireRate(rate);
            turret.UpdateBulletPrefab(prefab);
            turret.UpdateAttackRange(range);
            turret.UpdateTurretSprite(sprite);
            turret.UpdateTurretSound(sound);
        }
    }
    void OnBossDeath()
    {
        currentPhase = BossPhase.Dead;
        Debug.Log("Boss defeated!");

        if (playerWinMenu != null)
        {
            playerWinMenu.ShowPlayerWin();
        }
    }

}