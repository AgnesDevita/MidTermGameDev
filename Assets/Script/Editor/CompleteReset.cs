using UnityEngine;
using UnityEditor;

public class CompleteReset : EditorWindow
{
    [MenuItem("MidTerm Game/COMPLETE RESET - Fix Everything")]
    static void ShowWindow()
    {
        var window = GetWindow<CompleteReset>("Complete Reset");
        window.minSize = new Vector2(400, 300);
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox("COMPLETE RESET - START FRESH!", MessageType.Warning);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("RESET PLAYER - FIX EVERYTHING", GUILayout.Height(60)))
        {
            if (EditorUtility.DisplayDialog("Complete Reset", 
                "This will:\n\n" +
                "1. Remove ALL movement scripts\n" +
                "2. Add NEW simple controller\n" +
                "3. Fix Rigidbody settings\n" +
                "4. Uses OLD Input System (100% reliable!)\n\n" +
                "Continue?", 
                "YES - FIX IT!", "Cancel"))
            {
                CompleteResetPlayer();
            }
        }
        
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "After clicking the button:\n\n" +
            "1. Wait for script compilation\n" +
            "2. Save Scene (Ctrl+S)\n" +
            "3. Press Play\n" +
            "4. Use WASD + Mouse\n\n" +
            "This uses OLD Input System - NO Input System package issues!",
            MessageType.Info);
    }

    void CompleteResetPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "No Player found!", "OK");
            return;
        }

        Debug.Log("=== COMPLETE RESET STARTED ===");

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        int removed = 0;
        
        foreach (var script in scripts)
        {
            string typeName = script.GetType().Name;
            
            if (typeName == "PlayerController" || 
                typeName == "DirectMovementTest" || 
                typeName == "ForceRotationTest")
            {
                DestroyImmediate(script);
                Debug.Log($"Removed: {typeName}");
                removed++;
            }
        }

        var playerInput = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (playerInput != null)
        {
            DestroyImmediate(playerInput);
            Debug.Log("Removed: PlayerInput (New Input System)");
            removed++;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = 1f;
            rb.linearDamping = 2f;
            rb.angularDamping = 0f;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            Debug.Log("Fixed: Rigidbody settings");
        }

        Transform cameraTransform = player.transform.Find("Main Camera");
        
        SimplePlayerController spc = player.AddComponent<SimplePlayerController>();
        spc.walkSpeed = 10f;
        spc.runSpeed = 18f;
        spc.mouseSensitivity = 3f;
        spc.lookXLimit = 80f;
        
        if (cameraTransform != null)
        {
            spc.playerCamera = cameraTransform;
            Debug.Log("Linked: Main Camera");
        }

        EditorUtility.SetDirty(player);
        
        Debug.Log($"=== RESET COMPLETE ===");
        Debug.Log($"Removed {removed} old scripts");
        Debug.Log($"Added SimplePlayerController");
        Debug.Log($"Using OLD Input System (GetAxis)");
        
        EditorUtility.DisplayDialog("Reset Complete!", 
            $"Player reset successful!\n\n" +
            $"Removed: {removed} old scripts\n" +
            $"Added: SimplePlayerController\n\n" +
            $"NEXT STEPS:\n" +
            $"1. Save Scene (Ctrl+S)\n" +
            $"2. Press Play\n" +
            $"3. WASD to move\n" +
            $"4. Mouse to look\n" +
            $"5. Shift to run\n\n" +
            $"Check Console for green 'STARTED!' message!",
            "OK");
    }
}
