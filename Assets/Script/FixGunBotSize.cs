using UnityEngine;
using UnityEngine.AI;

public class FixGunBotSize : MonoBehaviour
{
    [Header("Click button in Inspector to fix size")]
    [Tooltip("Target effective radius in world units")]
    public float targetEffectiveRadius = 7f;
    
    [Tooltip("Auto-calculate based on scale")]
    public bool autoCalculate = true;

    [ContextMenu("Fix NavMeshAgent Size Now")]
    public void FixSize()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found!");
            return;
        }

        float scale = transform.localScale.x;
        float newRadius = targetEffectiveRadius / scale;
        float newHeight = (targetEffectiveRadius * 2.5f) / scale;

        agent.radius = newRadius;
        agent.height = newHeight;

        Debug.Log($"✅ Fixed GunBot size!\n" +
                  $"Scale: {scale}x\n" +
                  $"NavMesh Radius: {newRadius:F2} (Effective: {newRadius * scale:F1} units)\n" +
                  $"NavMesh Height: {newHeight:F2} (Effective: {newHeight * scale:F1} units)");

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            capsule.radius = newRadius;
            capsule.height = newHeight;
            Debug.Log($"✅ CapsuleCollider also updated to match NavMeshAgent");
        }
    }

    void OnValidate()
    {
        if (autoCalculate && Application.isEditor)
        {
            float scale = transform.localScale.x;
            float effectiveRadius = GetComponent<NavMeshAgent>()?.radius * scale ?? 0;
            
            if (effectiveRadius > 10f)
            {
                Debug.LogWarning($"⚠️ GunBot effective radius = {effectiveRadius:F1} units (TOO BIG!)\n" +
                                 $"Zombie can fit through doors but GunBot cannot.\n" +
                                 $"Click: Right-click this component → Fix NavMeshAgent Size Now");
            }
        }
    }
}
