using UnityEngine;

public interface IEnemyAI
{
    void SetTarget(Transform target);
    void Init(EnemyConfig config);
    void SetFirePoint(Transform firePoint);  // new method to assign firePoint dynamically
}
