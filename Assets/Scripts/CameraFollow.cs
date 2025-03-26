using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffSet;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(target.position.x, target.position.y + yOffSet, -10f);
        transform.position = Vector3.Slerp(transform.position, pos, followSpeed * Time.deltaTime);
    }
}
