using UnityEngine;
using UnityEditor;

public class DiagnoseRotation : EditorWindow
{
    [MenuItem("MidTerm Game/Diagnose Rotation Problem")]
    static void ShowWindow()
    {
        GetWindow<DiagnoseRotation>("Rotation Diagnosis");
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox("ROTATION DIAGNOSTIC TOOL", MessageType.Info);
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("1. CHECK RIGIDBODY CONSTRAINTS", GUILayout.Height(40)))
        {
            CheckConstraints();
        }
        
        if (GUILayout.Button("2. DISABLE ALL SCRIPTS EXCEPT PlayerController", GUILayout.Height(40)))
        {
            DisableOtherScripts();
        }
        
        if (GUILayout.Button("3. ADD FORCE ROTATION TEST", GUILayout.Height(40)))
        {
            AddForceRotationTest();
        }
        
        if (GUILayout.Button("4. REMOVE ALL ROTATION CONSTRAINTS", GUILayout.Height(40)))
        {
            RemoveAllConstraints();
        }
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "TEST STEPS:\n\n" +
            "1. Click button 1 to check constraints\n" +
            "2. Press Play\n" +
            "3. Move mouse & check Console\n" +
            "4. Press SPACE to force rotate 45°\n\n" +
            "Kalau SPACE gak work = Ada yang block rotation!",
            MessageType.Warning
        );
    }

    void CheckConstraints()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody on Player!");
            return;
        }

        Debug.Log("=== RIGIDBODY CONSTRAINTS CHECK ===");
        Debug.Log($"Constraints: {rb.constraints}");
        
        bool freezeX = (rb.constraints & RigidbodyConstraints.FreezeRotationX) != 0;
        bool freezeY = (rb.constraints & RigidbodyConstraints.FreezeRotationY) != 0;
        bool freezeZ = (rb.constraints & RigidbodyConstraints.FreezeRotationZ) != 0;
        
        Debug.Log($"Freeze Rotation X: {freezeX}");
        Debug.Log($"Freeze Rotation Y: {freezeY} ← SHOULD BE TRUE!");
        Debug.Log($"Freeze Rotation Z: {freezeZ}");
        
        Debug.Log($"Angular Drag: {rb.angularDamping}");
        Debug.Log($"Mass: {rb.mass}");
        
        if (!freezeY)
        {
            Debug.LogWarning("⚠️ Y ROTATION NOT FROZEN! Physics can interfere!");
        }
        
        if (rb.angularDamping > 0)
        {
            Debug.LogWarning($"⚠️ Angular Drag = {rb.angularDamping} (should be 0!)");
        }

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        Debug.Log($"\n=== SCRIPTS ON PLAYER ({scripts.Length}) ===");
        foreach (var script in scripts)
        {
            Debug.Log($"- {script.GetType().Name} (enabled: {script.enabled})");
        }
    }

    void DisableOtherScripts()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.GetType().Name != "PlayerController")
            {
                script.enabled = false;
                Debug.Log($"Disabled: {script.GetType().Name}");
            }
        }
        
        EditorUtility.DisplayDialog("Scripts Disabled", 
            "All scripts disabled except PlayerController!\n\nPress Play and test rotation.", 
            "OK");
    }

    void AddForceRotationTest()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        ForceRotationTest test = player.GetComponent<ForceRotationTest>();
        if (test == null)
        {
            test = player.AddComponent<ForceRotationTest>();
            Debug.Log("✅ Added ForceRotationTest!");
        }
        
        test.rotationSpeed = 100f;
        
        EditorUtility.DisplayDialog("Test Added", 
            "ForceRotationTest added!\n\n" +
            "IN PLAY MODE:\n" +
            "- Move mouse to rotate\n" +
            "- Press SPACE to force rotate 45°\n" +
            "- Check Console for logs\n\n" +
            "If SPACE doesn't rotate = something is blocking!",
            "OK");
    }

    void RemoveAllConstraints()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.constraints = RigidbodyConstraints.None;
        rb.angularDamping = 0f;
        
        Debug.LogWarning("⚠️ REMOVED ALL CONSTRAINTS! Player can flip/tumble now!");
        Debug.LogWarning("This is for testing only!");
        
        EditorUtility.DisplayDialog("Constraints Removed", 
            "ALL CONSTRAINTS REMOVED!\n\n" +
            "Player can now rotate freely.\n" +
            "Press Play and test if mouse rotation works.\n\n" +
            "⚠️ Player might flip/tumble!", 
            "OK");
    }
}
