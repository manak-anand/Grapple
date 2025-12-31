using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    [Header("Camera Limits")]
    public float minY = -3f;   // lowest the camera can go
    public float maxY = 8f;  // optional, if you ever want to cap the top

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 desired = target.position + offset;

        // clamp vertical camera movement
        desired.y = Mathf.Clamp(desired.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
}
