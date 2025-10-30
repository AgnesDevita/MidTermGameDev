using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 9f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 2f;
    public bool invertMouseY = false;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float rotationY = 0f;
    private float cameraRotationX = 0f;
    private bool isRunning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearDamping = 2f;
        rb.angularDamping = 0f;
        rb.mass = 1f;

        var navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }

        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
        }

        rotationY = transform.eulerAngles.y;

        if (cameraTransform != null && cameraTransform.parent == transform)
        {
            cameraRotationX = cameraTransform.localEulerAngles.x;
            if (cameraRotationX > 180f) cameraRotationX -= 360f;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRotation();
        isRunning = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (moveInput.magnitude > 0.01f)
        {
            Debug.Log($"<color=cyan>INPUT DETECTED: {moveInput}</color>");
        }

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveDirection * currentSpeed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
            
            Debug.Log($"<color=green>MOVING! Velocity={targetVelocity}</color>");
        }
        else
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.x = 0;
            velocity.z = 0;
            rb.linearVelocity = velocity;
        }
    }

    void HandleRotation()
    {
        if (Mouse.current == null || cameraTransform == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivityX;
        float mouseY = mouseDelta.y * mouseSensitivityY;

        if (invertMouseY) mouseY = -mouseY;

        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f);

        if (cameraTransform.parent == transform)
        {
            cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.LogWarning($"OnMove CALLED! Input: {moveInput}");
    }

    public void OnLook(InputValue value)
    {
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
}

