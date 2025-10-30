using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;

    [Header("Look")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float lookXLimit = 80f;

    private Rigidbody rb;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.mass = 1f;
        rb.linearDamping = 2f;
        rb.angularDamping = 0f;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        if (playerCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null) playerCamera = cam.transform;
        }

        rotationY = transform.eulerAngles.y;
        
        if (playerCamera != null && playerCamera.parent == transform)
        {
            rotationX = playerCamera.localEulerAngles.x;
            if (rotationX > 180f) rotationX -= 360f;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("<color=green>SimplePlayerController STARTED!</color>");
    }

    void Update()
    {
        HandleLook();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f)
        {
            Debug.Log($"<color=cyan>INPUT: H={horizontal:F2}, V={vertical:F2}</color>");
        }

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDir = (forward * vertical + right * horizontal).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        if (moveDir.magnitude > 0.1f)
        {
            Vector3 velocity = moveDir * speed;
            velocity.y = rb.linearVelocity.y;
            rb.linearVelocity = velocity;
            
            Debug.Log($"<color=green>MOVING! Speed={speed}, Velocity={velocity}</color>");
        }
        else
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.x = 0f;
            velocity.z = 0f;
            rb.linearVelocity = velocity;
        }
    }

    void HandleLook()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            Debug.Log($"<color=yellow>MOUSE: X={mouseX:F3}, Y={mouseY:F3}</color>");
        }

        rotationY += mouseX * mouseSensitivity;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        rotationX -= mouseY * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        if (playerCamera.parent == transform)
        {
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnGUI()
    {
        GUI.color = Color.green;
        GUI.Label(new Rect(10, 10, 300, 80), 
            "SIMPLE CONTROLLER ACTIVE\n" +
            "WASD = Move\n" +
            "Shift = Run\n" +
            "Mouse = Look");
    }
}
