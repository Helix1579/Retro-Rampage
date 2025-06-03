using UnityEngine;

[System.Serializable]
public class EnemyConfig
{
    public int damage = 1;

    // Patrol-specific
    public Transform pointA;
    public Transform pointB;

    // RangedChase-specific
    public float fireDistance = 4f;
}
