using UnityEngine;

public class PlayerShooter : Weapon, IShooter
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButton("Fire1"))
        {
            Vector2 direction = GetShootDirection();
            TryShoot(direction, "Player");
        }
    }

    private Vector2 GetShootDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0)
            return playerController.IsFacingRight ? Vector2.right : Vector2.left;

        return new Vector2(h, v).normalized;
    }

    public void Shoot(Vector2 direction)
    {
        TryShoot(direction, "Player");
    }
}
