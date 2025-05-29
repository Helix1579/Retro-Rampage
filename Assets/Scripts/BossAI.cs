using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossPhase { Phase1, Phase2, Phase3, Dead }
    private BossPhase currentPhase = BossPhase.Phase1;

    [Header("Combat Settings")]
    public Transform player;
    public float[] phaseFireRates = { 2f, 1.2f, 0.6f };

    [Header("Turrets")]
    public TurretController[] turrets;

    [Header("Phase Thresholds")]
    public int phase2Threshold = 70;
    public int phase3Threshold = 35;
    
    
    private EnemyHealth bossHealth;

    void Awake()
    {
        bossHealth = GetComponent<EnemyHealth>();
        bossHealth.OnDeathEvent += OnBossDeath;

        foreach (var turret in turrets)
            turret.Target = player;

        SetTurretsFireRate(phaseFireRates[0]);
    }

    void Update()
    {
        if (currentPhase == BossPhase.Dead || player == null) return;

        HandlePhaseTransition();
    }

    void HandlePhaseTransition()
    {
        int hp = bossHealth.CurrentHealth;

        if (hp <= 0)
        {
            EnterPhase(BossPhase.Dead);
            return;
        }

        if (hp <= phase3Threshold && currentPhase != BossPhase.Phase3)
            EnterPhase(BossPhase.Phase3);
        else if (hp <= phase2Threshold && currentPhase != BossPhase.Phase2)
            EnterPhase(BossPhase.Phase2);
    }

    void EnterPhase(BossPhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        Debug.Log($"Boss entered {newPhase}");

        if (newPhase == BossPhase.Dead)
        {
            SetTurretsFireRate(0f);
            return;
        }

        SetTurretsFireRate(phaseFireRates[(int)newPhase]);
    }

    void SetTurretsFireRate(float rate)
    {
        foreach (var turret in turrets)
        {
            turret.UpdateFireRate(rate);
        }
    }

    void OnBossDeath()
    {
        currentPhase = BossPhase.Dead;
        Debug.Log("Boss defeated!");
    }
}