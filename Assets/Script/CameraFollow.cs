using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Objek yang akan diikuti oleh kamera (drag & drop Zombie ke sini)")]
    public Transform target;

    [Header("Camera Positioning")]
    [Tooltip("Seberapa jauh kamera dari target")]
    public float distance = 5.0f;
    [Tooltip("Seberapa tinggi kamera dari target")]
    public float height = 2.0f; // PASTIKAN INI BUKAN 0 DI INSPECTOR
    [Tooltip("Seberapa mulus kamera mengikuti target")]
    public float smoothSpeed = 10.0f;

    [Header("Wall Collision")]
    [Tooltip("Layer yang dianggap sebagai penghalang (tembok, lantai, dll)")]
    public LayerMask collisionMask;
    [Tooltip("Seberapa dekat kamera bisa ke tembok sebelum berhenti")]
    public float collisionPadding = 0.35f;

    private Vector3 offset;
    private Vector3 desiredPosition;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: Target (player) belum di-set!");
            return;
        }
        // Menghitung offset awal kamera dari target
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Menentukan posisi ideal kamera di belakang target
        desiredPosition = target.position + (target.rotation * offset);

        // 2. Menghandle tabrakan dengan tembok menggunakan Raycast
        // Hitung collision SEBELUM lerping untuk hasil yang lebih smooth
        Vector3 finalPosition = HandleCollision();

        // 3. Mengatur posisi kamera dengan mulus (smooth)
        transform.position = Vector3.Lerp(transform.position, finalPosition, smoothSpeed * Time.deltaTime);

        // 4. Selalu membuat kamera melihat ke arah target
        // Kita LookAt sedikit ke atas kepala target (1.0f), BUKAN di kaki (0)
        transform.LookAt(target.position + Vector3.up * 1.0f); 
    }

    private Vector3 HandleCollision()
    {
        RaycastHit hit;
        // Titik awal raycast juga sedikit di atas pivot (kaki)
        Vector3 targetPosition = target.position + Vector3.up * 1.0f;
        
        // Hitung direction dari target ke desired camera position
        Vector3 direction = (desiredPosition - targetPosition).normalized;
        float distanceToCamera = Vector3.Distance(targetPosition, desiredPosition);
        
        // Raycast dari target ke posisi kamera yang diinginkan
        if (Physics.Raycast(targetPosition, direction, out hit, distanceToCamera, collisionMask))
        {
            // Jika ada collision, posisikan kamera di titik collision dengan padding
            return hit.point + hit.normal * collisionPadding;
        }
        
        // Jika tidak ada collision, return desired position
        return desiredPosition;
    }
}