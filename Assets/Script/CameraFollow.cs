using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Offset")]
    public Transform target; // Target yang diikuti (Zombie)
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0); // Titik yang dilihat kamera (setinggi dada/kepala)

    [Header("Camera Settings")]
    public float mouseSensitivity = 2.0f;
    public float distanceFromTarget = 6.0f; // Jarak ideal kamera dari target
    public Vector2 pitchMinMax = new Vector2(-10, 80); // Batas kamera melihat ke atas/bawah
    public float rotationSmoothTime = 0.1f;

    [Header("Collision")]
    public LayerMask collisionMask; // Layer untuk tembok dan objek lain yang harus dihindari
    public float collisionPadding = 0.2f; // Jarak aman dari tembok agar tidak tembus

    // Variabel internal
    private Vector2 lookInput;
    private float yaw; // Rotasi horizontal
    private float pitch; // Rotasi vertikal
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            pitch = 15.0f;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Mengambil input mouse untuk rotasi
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // 2. Menghaluskan pergerakan rotasi kamera
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        Quaternion rotation = Quaternion.Euler(currentRotation);

        // 3. Menghitung posisi ideal kamera (jika tidak ada tembok)
        Vector3 desiredPosition = target.position + targetOffset + rotation * new Vector3(0, 0, -distanceFromTarget);

        // 4. Cek Tembok (Collision)
        RaycastHit hit;
        // Kita "menembakkan" garis dari target ke posisi ideal kamera
        if (Physics.Linecast(target.position + targetOffset, desiredPosition, out hit, collisionMask))
        {
            // Jika garisnya menabrak sesuatu, pindahkan kamera ke titik tabrakan
            transform.position = hit.point + hit.normal * collisionPadding;
        }
        else
        {
            // Jika aman, tempatkan kamera di posisi ideal
            transform.position = desiredPosition;
        }

        // 5. Buat kamera selalu melihat ke arah target
        transform.LookAt(target.position + targetOffset);
    }
}