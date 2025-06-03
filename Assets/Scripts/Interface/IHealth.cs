using UnityEngine;

public interface IHealth
{
    void TakeDamage(int amount);
    void Heal(int amount);
}
