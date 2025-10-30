using UnityEngine;

public class DirectMovementTest : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.LogError("NO RIGIDBODY!");
            return;
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        Debug.LogWarning("DIRECT MOVEMENT TEST ACTIVE!");
        Debug.LogWarning("Use ARROW KEYS to move!");
        Debug.LogWarning("Use Q/E to rotate!");
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f)
        {
            Debug.Log($"<color=yellow>INPUT: H={h:F2}, V={v:F2}</color>");
        }

        Vector3 moveDir = new Vector3(h, 0, v).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            Vector3 vel = moveDir * speed;
            vel.y = rb.linearVelocity.y;
            rb.linearVelocity = vel;
            
            Debug.Log($"<color=green>MOVING! Velocity={vel}</color>");
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.fixedDeltaTime);
            Debug.Log("ROTATING LEFT");
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
            Debug.Log("ROTATING RIGHT");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            Debug.Log("JUMP!");
        }
    }

    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Label(new Rect(10, 10, 400, 100), 
            "DIRECT MOVEMENT TEST\n" +
            "ARROW KEYS = Move\n" +
            "Q/E = Rotate\n" +
            "SPACE = Jump");
    }
}
