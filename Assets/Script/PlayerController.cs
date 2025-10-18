using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 2f;
    public bool invertMouseY = false;
    
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraRotationX = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        var navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.enabled = false;
            Debug.Log("NavMeshAgent disabled by PlayerController");
        }
        
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
        }
        
        if (cameraTransform != null && cameraTransform.parent == transform)
        {
            cameraRotationX = cameraTransform.localEulerAngles.x;
            Debug.Log("Camera is child of player - using simple parent-child camera");
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleRotation();
        HandleCamera();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();
        
        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();
        
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveDirection * moveSpeed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
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
        if (moveInput.magnitude > 0.1f)
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();
            
            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();
            
            Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
            
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void HandleCamera()
    {
        if (cameraTransform == null) return;
        
        if (cameraTransform.parent == transform)
        {
            float mouseY = lookInput.y * mouseSensitivityY;
            if (invertMouseY) mouseY = -mouseY;
            
            cameraRotationX -= mouseY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -30f, 60f);
            
            cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
            
            float mouseX = lookInput.x * mouseSensitivityX;
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log($"OnMove called! Input: {moveInput}");
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        Debug.Log($"OnLook called! Input: {lookInput}");
    }
}
