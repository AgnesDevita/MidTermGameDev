using UnityEngine;
using UnityEditor;

public class AdjustSpeed : EditorWindow
{
    private float walkSpeed = 10f;
    private float runSpeed = 18f;
    private float mouseSens = 3f;

    [MenuItem("MidTerm Game/Adjust Player Speed")]
    static void ShowWindow()
    {
        var window = GetWindow<AdjustSpeed>("Speed Adjuster");
        window.minSize = new Vector2(350, 250);
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox("ADJUST PLAYER SPEED", MessageType.Info);
        
        GUILayout.Space(10);
        
        EditorGUILayout.LabelField("Movement Speed:", EditorStyles.boldLabel);
        walkSpeed = EditorGUILayout.Slider("Walk Speed", walkSpeed, 1f, 30f);
        runSpeed = EditorGUILayout.Slider("Run Speed (Shift)", runSpeed, 5f, 50f);
        
        GUILayout.Space(10);
        
        EditorGUILayout.LabelField("Mouse Look:", EditorStyles.boldLabel);
        mouseSens = EditorGUILayout.Slider("Mouse Sensitivity", mouseSens, 0.5f, 10f);
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("APPLY SPEED SETTINGS", GUILayout.Height(50)))
        {
            ApplySpeed();
        }
        
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "PRESETS:\n" +
            "Slow: Walk=5, Run=9\n" +
            "Normal: Walk=10, Run=18\n" +
            "Fast: Walk=15, Run=25\n" +
            "Very Fast: Walk=20, Run=35",
            MessageType.None);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Slow"))
        {
            walkSpeed = 5f;
            runSpeed = 9f;
            mouseSens = 2f;
        }
        if (GUILayout.Button("Normal"))
        {
            walkSpeed = 10f;
            runSpeed = 18f;
            mouseSens = 3f;
        }
        if (GUILayout.Button("Fast"))
        {
            walkSpeed = 15f;
            runSpeed = 25f;
            mouseSens = 4f;
        }
        if (GUILayout.Button("Very Fast"))
        {
            walkSpeed = 20f;
            runSpeed = 35f;
            mouseSens = 5f;
        }
        EditorGUILayout.EndHorizontal();
    }

    void ApplySpeed()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Player not found!", "OK");
            return;
        }

        SimplePlayerController spc = player.GetComponent<SimplePlayerController>();
        if (spc == null)
        {
            EditorUtility.DisplayDialog("Error", 
                "SimplePlayerController not found on Player!\n\n" +
                "Run 'COMPLETE RESET' first!",
                "OK");
            return;
        }

        spc.walkSpeed = walkSpeed;
        spc.runSpeed = runSpeed;
        spc.mouseSensitivity = mouseSens;

        EditorUtility.SetDirty(player);

        Debug.Log($"<color=green>SPEED UPDATED!</color>");
        Debug.Log($"Walk Speed: {walkSpeed}");
        Debug.Log($"Run Speed: {runSpeed}");
        Debug.Log($"Mouse Sensitivity: {mouseSens}");

        EditorUtility.DisplayDialog("Speed Updated!", 
            $"Player speed updated!\n\n" +
            $"Walk: {walkSpeed}\n" +
            $"Run: {runSpeed}\n" +
            $"Mouse: {mouseSens}\n\n" +
            $"Press Play to test!",
            "OK");
    }
}
