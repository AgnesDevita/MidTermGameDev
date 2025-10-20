using UnityEngine;

// Pastikan komponen ini ada di object Zombie Anda
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Kecepatan berjalan normal")]
    public float walkSpeed = 3.0f;
    
    [Tooltip("Kecepatan saat berlari")]
    public float runSpeed = 6.0f;
    
    [Tooltip("Seberapa cepat karakter berputar (dalam derajat per detik)")]
    public float rotationSpeed = 200.0f;

    private Rigidbody rb;
    private Animator animator;
    private float currentSpeed = 0f;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Mengunci rotasi pada sumbu X dan Z agar karakter tidak terguling
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Jika sudah mati, hentikan semua proses input dan pergerakan
        if (isDead) return;

        HandleMovement();
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        // 1. Mengambil Input dari Keyboard (W/A/S/D atau Panah)
        float verticalInput = Input.GetAxis("Vertical"); // Maju (W/Up) dan Mundur (S/Down)
        float horizontalInput = Input.GetAxis("Horizontal"); // Belok Kiri (A/Left) dan Kanan (D/Right)

        // 2. Menentukan kecepatan berdasarkan tombol Lari (Left Shift)
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 3. Menghitung Vektor Gerakan
        // Gerakan maju/mundur hanya jika ada input vertikal
        Vector3 movement = transform.forward * verticalInput * currentSpeed * Time.deltaTime;

        // 4. Mengaplikasikan Gerakan
        // Kita menggunakan rb.MovePosition agar interaksi dengan fisika lebih baik
        rb.MovePosition(rb.position + movement);

        // 5. Menghitung dan Mengaplikasikan Rotasi/Belok
        float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }

    private void UpdateAnimations()
    {
        // Mengambil input vertikal absolut untuk menentukan apakah ada gerakan maju/mundur
        float moveMagnitude = Mathf.Abs(Input.GetAxis("Vertical"));
        
        // Jika sedang berlari, magnitude animasinya lebih besar
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Jika ada input gerak maju/mundur
        if(moveMagnitude > 0.1f)
        {
            // Set speed animator ke 2 jika lari, atau 1 jika jalan
            animator.SetFloat("Speed", isRunning ? 2.0f : 1.0f);
        }
        else
        {
            // Jika tidak ada input, speed animator 0 (Idle)
            animator.SetFloat("Speed", 0f);
        }
    }

    // --- FUNGSI UNTUK DIPANGGIL DARI SCRIPT LAIN (CONTOH: GUNBOT) ---
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("Die");
            
            // Menonaktifkan collider agar tidak menghalangi jalan
            GetComponent<CapsuleCollider>().enabled = false;
            rb.isKinematic = true; // Hentikan Rigidbody dari pengaruh fisika
            Debug.Log("Player has died!");
        }
    }
}