using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float lerpSpeed = 0.05f;

    private float targetValue;

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
    }

    public void SetHealth(int currentHealth)
    {
        targetValue = currentHealth;
        healthSlider.value = currentHealth; // instant update
    }

    void Update()
    {
        if (easeHealthSlider.value != targetValue)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, targetValue, lerpSpeed);
        }
    }
}