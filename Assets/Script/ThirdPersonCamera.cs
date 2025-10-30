using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);

    [Header("Camera Settings")]
    public float distance = 10f;
    public float height = 5f;
    public float smoothSpeed = 10f;
    public float rotationSpeed = 5f;

    [Header("Mouse Look")]
    public bool enableMouseRotation = true;
    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 2f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    [Header("Collision")]
    public bool avoidObstacles = true;
    public LayerMask collisionMask;
    public float collisionRadius = 0.3f;

    private float currentX = 0f;
    private float currentY = 20f;
    private Vector3 currentVelocity;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            currentX = angles.y;
            currentY = angles.x;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (enableMouseRotation && Cursor.lockState == CursorLockMode.Locked)
        {
            currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
        }

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);
        desiredPosition.y = target.position.y + height;

        if (avoidObstacles)
        {
            Vector3 direction = desiredPosition - target.position;
            if (Physics.SphereCast(target.position, collisionRadius, direction.normalized, 
                out RaycastHit hit, direction.magnitude, collisionMask))
            {
                desiredPosition = target.position + direction.normalized * (hit.distance - collisionRadius);
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, 
            ref currentVelocity, 1f / smoothSpeed);
        
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, 1f);
        Gizmos.DrawLine(target.position, transform.position);
    }
}
