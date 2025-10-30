using UnityEngine;
using UnityEditor;

public class EmergencyFix : EditorWindow
{
    [MenuItem("MidTerm Game/EMERGENCY FIX - Player Stuck")]
    static void ShowWindow()
    {
        GetWindow<EmergencyFix>("Emergency Fix");
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox("PLAYER STUCK - EMERGENCY DIAGNOSTIC", MessageType.Error);
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("1. ADD DIRECT MOVEMENT TEST (Bypass Input System)", GUILayout.Height(50)))
        {
            AddDirectMovement();
        }
        
        if (GUILayout.Button("2. CHECK & FIX RIGIDBODY", GUILayout.Height(50)))
        {
            CheckRigidbody();
        }
        
        if (GUILayout.Button("3. REMOVE ALL COLLIDERS (Test Physics)", GUILayout.Height(50)))
        {
            RemoveColliders();
        }
        
        if (GUILayout.Button("4. DISABLE ALL SCRIPTS (Except Test)", GUILayout.Height(50)))
        {
            DisableAllScripts();
        }

        if (GUILayout.Button("5. TELEPORT UP (Get Unstuck)", GUILayout.Height(50)))
        {
            TeleportUp();
        }
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "STEPS:\n\n" +
            "1. Click button 1 - Add Direct Movement Test\n" +
            "2. Press Play\n" +
            "3. Use ARROW KEYS to move\n" +
            "4. If it works = Input System problem\n" +
            "5. If still stuck = Physics/Collider problem",
            MessageType.Info
        );
    }

    void AddDirectMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Player not found!", "OK");
            return;
        }

        DirectMovementTest test = player.GetComponent<DirectMovementTest>();
        if (test == null)
        {
            test = player.AddComponent<DirectMovementTest>();
        }
        
        test.speed = 5f;
        test.rotationSpeed = 100f;

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.GetType().Name == "PlayerController" || 
                script.GetType().Name == "PlayerAnimation")
            {
                script.enabled = false;
                Debug.Log($"Disabled: {script.GetType().Name}");
            }
        }

        EditorUtility.SetDirty(player);
        
        EditorUtility.DisplayDialog("Test Added!", 
            "DirectMovementTest added!\n\n" +
            "Press Play and use:\n" +
            "- ARROW KEYS to move\n" +
            "- Q/E to rotate\n" +
            "- SPACE to jump\n\n" +
            "Check Console for logs!",
            "OK");
    }

    void CheckRigidbody()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            EditorUtility.DisplayDialog("Error", "No Rigidbody on Player!", "OK");
            return;
        }

        Debug.Log("=== RIGIDBODY CHECK ===");
        Debug.Log($"Is Kinematic: {rb.isKinematic}");
        Debug.Log($"Use Gravity: {rb.useGravity}");
        Debug.Log($"Mass: {rb.mass}");
        Debug.Log($"Drag: {rb.linearDamping}");
        Debug.Log($"Angular Drag: {rb.angularDamping}");
        Debug.Log($"Constraints: {rb.constraints}");
        Debug.Log($"Position: {player.transform.position}");
        Debug.Log($"Velocity: {rb.linearVelocity}");

        bool wasFixed = false;

        if (rb.isKinematic)
        {
            rb.isKinematic = false;
            Debug.LogWarning("FIXED: Was kinematic!");
            wasFixed = true;
        }

        if (!rb.useGravity)
        {
            rb.useGravity = true;
            Debug.LogWarning("FIXED: Gravity was off!");
            wasFixed = true;
        }

        if (rb.mass < 0.1f)
        {
            rb.mass = 1f;
            Debug.LogWarning("FIXED: Mass was too low!");
            wasFixed = true;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        EditorUtility.SetDirty(player);

        if (wasFixed)
        {
            EditorUtility.DisplayDialog("Rigidbody Fixed!", 
                "Check Console for details.\n\nPress Play and test!", 
                "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Rigidbody OK", 
                "Rigidbody settings look fine.\n\nCheck Console for details.", 
                "OK");
        }
    }

    void RemoveColliders()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Collider[] colliders = player.GetComponents<Collider>();
        
        if (colliders.Length == 0)
        {
            EditorUtility.DisplayDialog("No Colliders", "Player has no colliders!", "OK");
            return;
        }

        foreach (var col in colliders)
        {
            DestroyImmediate(col);
            Debug.LogWarning($"Removed: {col.GetType().Name}");
        }

        CapsuleCollider newCol = player.AddComponent<CapsuleCollider>();
        newCol.height = 2f;
        newCol.radius = 0.5f;
        newCol.center = new Vector3(0, 1f, 0);

        EditorUtility.SetDirty(player);
        
        EditorUtility.DisplayDialog("Colliders Reset", 
            "Old colliders removed.\nNew CapsuleCollider added.\n\nPress Play and test!",
            "OK");
    }

    void DisableAllScripts()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.GetType().Name != "DirectMovementTest")
            {
                script.enabled = false;
                Debug.Log($"Disabled: {script.GetType().Name}");
            }
        }

        EditorUtility.DisplayDialog("Scripts Disabled", 
            "All scripts disabled except DirectMovementTest.\n\nPress Play and test!",
            "OK");
    }

    void TeleportUp()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        player.transform.position = new Vector3(0, 10f, 0);
        
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        EditorUtility.SetDirty(player);
        
        Debug.LogWarning("Player teleported to (0, 10, 0)");
        
        EditorUtility.DisplayDialog("Teleported!", 
            "Player moved to Y=10\n\nSave scene if you want to keep this position.",
            "OK");
    }
}
