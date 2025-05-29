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

    void Awake()
    {
        bossHealth = GetComponent<EnemyHealth>();
        bossHealth.OnDeathEvent += OnBossDeath;

        foreach (var turret in turrets)
            turret.Target = player;

        bossHp = bossHealth.CurrentHealth;
        SetTurretsFireRate(phaseFireRates[0], bulletPrefabs[0], phaseAttackRange[0], turretSprites[0]);
    }

    void Update()
    {
        if (currentPhase == BossPhase.Dead || player == null) return;

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