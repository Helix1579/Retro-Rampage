using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponUI : MonoBehaviour
{
    [Header("UI References")]
    public Image weaponIcon;

    public void UpdateWeapon(Sprite icon)
    {
        Debug.Log("Updating weapon icon: " + (icon != null ? icon.name : "null"));
        if (weaponIcon != null)
        {
            weaponIcon.sprite = icon;
            weaponIcon.enabled = icon != null;
        }
    }
}
