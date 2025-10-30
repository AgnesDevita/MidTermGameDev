using UnityEngine;
using UnityEngine.InputSystem;

public class ForceRotationTest : MonoBehaviour
{
    public float rotationSpeed = 100f;
    
    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            
            if (Mathf.Abs(mouseDelta.x) > 0.1f)
            {
                float rotation = mouseDelta.x * rotationSpeed * Time.deltaTime;
                
                Debug.LogWarning($"FORCE ROTATION TEST: MouseDelta={mouseDelta.x:F2}, Rotation={rotation:F4}");
                Debug.LogWarning($"BEFORE: Y={transform.eulerAngles.y:F2}");
                
                Vector3 currentEuler = transform.eulerAngles;
                currentEuler.y += rotation;
                transform.eulerAngles = currentEuler;
                
                Debug.LogWarning($"AFTER: Y={transform.eulerAngles.y:F2}");
            }
        }
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.LogWarning("SPACE PRESSED - Manual rotate 45 degrees!");
            transform.Rotate(Vector3.up * 45f);
        }
    }
}
