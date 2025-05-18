using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : Weapon, IShooter
{
    [Header("Weapon System")]
    public WeaponData baseWeapon;
    public List<WeaponData> unlockedWeapons = new List<WeaponData>();

    private int currentWeaponIndex = 0;
    private float nextFireTime = 0f;

    [Header("UI Reference")]
    public WeaponUI weaponUI;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        if (baseWeapon != null && unlockedWeapons.Count == 0)
        {
            unlockedWeapons.Add(baseWeapon);
            UpdateWeaponUI();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Vector2 direction = GetShootDirection();
            TryShoot(direction, "Player");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SwitchWeapon();
        }
    }

    public void UnlockWeapon(WeaponData weapon)
    {
        if (!unlockedWeapons.Contains(weapon))
        {
            unlockedWeapons.Add(weapon);
            Debug.Log($"Unlocked weapon: {weapon.weaponName}");

            // Auto-select the newly unlocked weapon
            currentWeaponIndex = unlockedWeapons.Count - 1;
            UpdateWeaponUI();
        }
    }
    
    public void ResetWeapons()
    {
        unlockedWeapons.Clear();
    
        if (baseWeapon != null)
        {
            unlockedWeapons.Add(baseWeapon);
            currentWeaponIndex = 0;
            UpdateWeaponUI();
            Debug.Log("Weapons reset to base weapon.");
        }
        else
        {
            Debug.LogWarning("Base weapon is not assigned, cannot reset weapons.");
        }
    }


    private void SwitchWeapon()
    {
        if (unlockedWeapons.Count <= 1) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % unlockedWeapons.Count;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        if (weaponUI != null && unlockedWeapons.Count > 0)
        {
            weaponUI.UpdateWeapon(unlockedWeapons[currentWeaponIndex].weaponName);
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

    public override void TryShoot(Vector2 direction, string shooterTag)
    {
        if (unlockedWeapons.Count == 0) return;

        WeaponData weapon = unlockedWeapons[currentWeaponIndex];
        nextFireTime = Time.time + weapon.fireRate;

        GameObject bulletObj = Instantiate(weapon.bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction, bulletSpeed, shooterTag);
            bullet.damage = weapon.damage;
        }

        Destroy(bulletObj, bulletLifeTime);
    }

    public void Shoot(Vector2 direction)
    {
        TryShoot(direction, "Player");
    }
}
