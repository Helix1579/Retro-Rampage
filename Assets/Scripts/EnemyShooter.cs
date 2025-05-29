using UnityEngine;

public class EnemyShooter : Weapon, IShooter
{
    public Transform player;
    public float attackRange = 6f;
    private EnemyConfig currentConfig;
    public void SetConfig(EnemyConfig config) => currentConfig = config;

    private void Start()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            Shoot(direction);
        }
    }

    public void Shoot(Vector2 direction)
    {
        TryShoot(direction, "Enemy");
    }

}
