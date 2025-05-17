using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [Header("Text UI Reference")]
    public TextMeshProUGUI weaponText;

    public void UpdateWeapon(string weaponName)
    {
        if (weaponText != null)
        {
            weaponText.text = $"Weapon: {weaponName}";
        }
    }
}
