using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 20f;

    // Speed at which the camera smoothly interpolates to the desired position.
    [SerializeField]
    private float transitionSpeed = 2f;

    // The target position that navigation buttons update.
    private Vector3 desiredPosition;
    Vector2 deltaMovement; 

    private void Awake()
    {
        // Start with the camera at its current world position.
        desiredPosition = transform.position;
    }

    public void UpdateDeltaMovement(Vector2 newDelta)
    {
        deltaMovement = newDelta;
    }

    private void Update()
    {
        if(deltaMovement != Vector2.zero)
        {
            desiredPosition += new Vector3(deltaMovement.x * -1, 0f, deltaMovement.y * -1) * moveDistance * Time.deltaTime;
        }
        // Smoothly move towards the desired position each frame.
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * transitionSpeed);
    }
}
