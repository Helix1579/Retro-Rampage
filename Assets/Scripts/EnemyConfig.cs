using UnityEngine;

[System.Serializable]
public class EnemyConfig
{
    public int damage = 1;
    public float fireRate = 1.5f;
    public float detectionRange = 5f;

    // Patrol-specific
    public Transform pointA;
    public Transform pointB;

    // RangedChase-specific
    public float fireDistance = 4f;
}
