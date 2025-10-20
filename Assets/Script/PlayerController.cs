// Ganti semua isi PlayerController.cs dengan kode ini
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))] // Pastikan ada komponen Animator
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f; // Ditingkatkan agar rotasi lebih responsif

    [Header("References")]
    public Transform cameraTransform; // Referensi ke transform kamera utama

    private Rigidbody rb;
    private Animator animator; // Variabel untuk Animator
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Pastikan Rigidbody tidak berputar sendiri
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Mencari kamera utama secara otomatis jika belum di-set
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }
    
    void Update()
    {
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        // Arah depan kamera (tanpa komponen Y)
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // Arah kanan kamera (tanpa komponen Y)
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Hitung arah pergerakan berdasarkan input dan arah kamera
        Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Gerakkan Rigidbody
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // Rotasi karakter agar menghadap ke arah gerakan
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimator()
    {
        // Mengirim informasi kecepatan ke Animator
        // Magnitude dari input (0 jika diam, 1 jika bergerak)
        float speed = moveInput.magnitude;
        animator.SetFloat("Speed", speed);
    }


    // Fungsi ini dipanggil oleh Player Input Component
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}