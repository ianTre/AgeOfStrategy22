using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if(target == null) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }

    public void MoveHorizontal(float amount)
    {
        Vector3 targetPosition = transform.position + new Vector3(amount, 0f, 0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
