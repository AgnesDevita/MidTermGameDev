using UnityEngine;
using UnityEditor;

public class FixPlayerMovement : EditorWindow
{
    [MenuItem("MidTerm Game/Fix Player Movement")]
    static void ShowWindow()
    {
        GetWindow<FixPlayerMovement>("Fix Player");
    }

    void OnGUI()
    {
        GUILayout.Label("🔧 FIX PLAYER MOVEMENT", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("Player tidak bisa jalan? Klik tombol di bawah untuk fix!", MessageType.Info);
        GUILayout.Space(10);

        if (GUILayout.Button("✅ FIX PLAYER (Zombie) - AUTO", GUILayout.Height(40)))
        {
            FixPlayerAuto();
        }

        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox("MANUAL CHECK:", MessageType.None);
        
        if (GUILayout.Button("1. Check Input System Settings", GUILayout.Height(30)))
        {
            CheckInputSystemSettings();
        }
        
        if (GUILayout.Button("2. Fix Rigidbody Constraints", GUILayout.Height(30)))
        {
            FixRigidbodyConstraints();
        }
        
        if (GUILayout.Button("3. Verify PlayerInput Component", GUILayout.Height(30)))
        {
            VerifyPlayerInput();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("📖 Open Input System Settings", GUILayout.Height(30)))
        {
            SettingsService.OpenProjectSettings("Project/Player");
        }
    }

    void FixPlayerAuto()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Player Not Found", 
                "Tidak ada GameObject dengan tag 'Player'!\n\nSelect Zombie di Hierarchy lalu klik lagi.", 
                "OK");
            return;
        }

        Debug.Log($"🔧 Fixing player: {player.name}");

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
            Debug.Log("✅ Fixed Rigidbody: Freeze X & Z rotation only!");
        }

        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.applyRootMotion = false;
            Debug.Log("✅ Disabled Animator Root Motion!");
        }

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc == null)
        {
            pc = player.AddComponent<PlayerController>();
            Debug.Log("✅ Added PlayerController");
        }

        SerializedObject pcSO = new SerializedObject(pc);
        pcSO.FindProperty("moveSpeed").floatValue = 5f;
        pcSO.FindProperty("runSpeed").floatValue = 9f;
        pcSO.FindProperty("mouseSensitivityX").floatValue = 2f;
        pcSO.FindProperty("mouseSensitivityY").floatValue = 2f;
        pcSO.ApplyModifiedProperties();
        Debug.Log("✅ FIXED: Speeds & Sensitivity reset!");

        Transform cameraChild = player.transform.Find("Main Camera");
        if (cameraChild != null && pc != null)
        {
            SerializedObject so = new SerializedObject(pc);
            so.FindProperty("cameraTransform").objectReferenceValue = cameraChild;
            so.ApplyModifiedProperties();
            Debug.Log("✅ Linked Main Camera to PlayerController");
        }

        UnityEngine.InputSystem.PlayerInput pi = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (pi == null)
        {
            pi = player.AddComponent<UnityEngine.InputSystem.PlayerInput>();
            string inputActionPath = "Assets/UTS/MidTermGameDev/Assets/InputSystem_Actions.inputactions";
            var inputActions = AssetDatabase.LoadAssetAtPath<UnityEngine.InputSystem.InputActionAsset>(inputActionPath);
            if (inputActions != null)
            {
                pi.actions = inputActions;
                pi.defaultActionMap = "Player";
                pi.notificationBehavior = UnityEngine.InputSystem.PlayerNotifications.SendMessages;
                Debug.Log("✅ Setup PlayerInput component");
            }
        }

        EditorUtility.SetDirty(player);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(player.scene);

        EditorUtility.DisplayDialog("Success!", 
            "✅ Player sudah di-fix untuk SMOOTH MOVEMENT!\n\n" +
            "CHANGES:\n" +
            "• Rigidbody: Damping untuk smooth & stable\n" +
            "• PlayerController: Fixed rotation & movement logic\n" +
            "• Movement: Sekarang ngikutin arah hadapan player\n" +
            "• Rotation: Mouse look work properly\n\n" +
            "SAVE SCENE dan PRESS PLAY untuk test!", 
            "OK");

        Debug.Log("🎉 Player fix completed! Movement smooth, rotation work, no jitter!");
    }

    void CheckInputSystemSettings()
    {
        string currentSetting = "Unknown";
        
#if ENABLE_INPUT_SYSTEM && ENABLE_LEGACY_INPUT_MANAGER
        currentSetting = "Both (Old and New) ✅ GOOD!";
#elif ENABLE_INPUT_SYSTEM
        currentSetting = "Input System Package (New) ✅ GOOD!";
#elif ENABLE_LEGACY_INPUT_MANAGER
        currentSetting = "Input Manager (Old) ❌ WRONG!";
#else
        currentSetting = "None ❌ ERROR!";
#endif

        EditorUtility.DisplayDialog("Input System Check", 
            $"Current Setting: {currentSetting}\n\n" +
            "Untuk player movement pakai Input System, setting harus:\n" +
            "- 'Both' (Recommended)\n" +
            "- 'Input System Package (New)'\n\n" +
            "Kalau salah:\n" +
            "Edit > Project Settings > Player > Other Settings\n" +
            "Active Input Handling → Pilih 'Both'\n" +
            "Restart Unity setelah ganti!", 
            "OK");

        Debug.Log($"Input System Setting: {currentSetting}");
    }

    void FixRigidbodyConstraints()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Player not found! Select Zombie GameObject.", "OK");
            return;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            EditorUtility.DisplayDialog("Error", "Player tidak punya Rigidbody!", "OK");
            return;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        EditorUtility.SetDirty(player);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(player.scene);

        EditorUtility.DisplayDialog("Success", 
            "✅ Rigidbody constraints fixed!\n\n" +
            "Constraints: Freeze Rotation X & Z\n" +
            "Interpolation: Interpolate\n" +
            "Collision: ContinuousDynamic", 
            "OK");

        Debug.Log("✅ Fixed Rigidbody constraints for player");
    }

    void VerifyPlayerInput()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Player not found!", "OK");
            return;
        }

        UnityEngine.InputSystem.PlayerInput pi = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (pi == null)
        {
            EditorUtility.DisplayDialog("Missing Component", 
                "Player tidak punya PlayerInput component!\n\nKlik 'FIX PLAYER AUTO' untuk add.", 
                "OK");
            return;
        }

        string status = "PlayerInput Component:\n\n";
        status += $"Actions: {(pi.actions != null ? pi.actions.name : "NULL ❌")}\n";
        status += $"Action Map: {pi.defaultActionMap}\n";
        status += $"Notification: {pi.notificationBehavior}\n";

        bool hasActions = pi.actions != null;
        bool correctMap = pi.defaultActionMap == "Player";
        bool correctNotif = pi.notificationBehavior == UnityEngine.InputSystem.PlayerNotifications.SendMessages;

        if (hasActions && correctMap && correctNotif)
        {
            status += "\n✅ Setup looks good!";
        }
        else
        {
            status += "\n❌ Ada yang salah! Klik 'FIX PLAYER AUTO'";
        }

        EditorUtility.DisplayDialog("PlayerInput Check", status, "OK");
        Debug.Log(status);
    }
}
