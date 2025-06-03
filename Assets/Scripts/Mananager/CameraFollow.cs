using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float fixedY = 0f;         // Set this in the Inspector to the Y height of your level
    public Transform target;

    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, fixedY, -10f);
        transform.position = Vector3.Slerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}