using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class ProjectValidator : EditorWindow
{
    private Vector2 scrollPos;
    private bool showPassed = true;
    private bool showFailed = true;
    private bool showWarnings = true;

    private List<ValidationResult> results = new List<ValidationResult>();

    [MenuItem("MidTerm Game/Validate Project Setup")]
    static void ShowWindow()
    {
        var window = GetWindow<ProjectValidator>("Project Validator");
        window.minSize = new Vector2(500, 600);
        window.RunValidation();
    }

    void OnGUI()
    {
        GUILayout.Label("🔍 PROJECT VALIDATION REPORT", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("🔄 Run Validation", GUILayout.Height(30)))
        {
            RunValidation();
        }
        if (GUILayout.Button("📖 Open Setup Guide", GUILayout.Height(30)))
        {
            OpenSetupGuide();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        showPassed = EditorGUILayout.ToggleLeft("✅ Show Passed", showPassed, GUILayout.Width(150));
        showFailed = EditorGUILayout.ToggleLeft("❌ Show Failed", showFailed, GUILayout.Width(150));
        showWarnings = EditorGUILayout.ToggleLeft("⚠️ Show Warnings", showWarnings, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var result in results)
        {
            if (result.status == ValidationStatus.Pass && !showPassed) continue;
            if (result.status == ValidationStatus.Fail && !showFailed) continue;
            if (result.status == ValidationStatus.Warning && !showWarnings) continue;

            DrawValidationResult(result);
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        DrawSummary();
    }

    void RunValidation()
    {
        results.Clear();

        ValidateScripts();
        ValidateScenes();
        ValidateInputSystem();
        ValidateTagsAndLayers();
        ValidateCurrentScene();

        Repaint();
    }

    void ValidateScripts()
    {
        AddHeader("📜 SCRIPT VALIDATION");

        string[] requiredScripts = new string[]
        {
            "GameManager", "SceneLoader", "GameProgress", "LevelConfig",
            "PlayerController", "PlayerHealth", "ThirdPersonCamera",
            "GunBotAI", "Diamond", "DiamondSpawner",
            "GameUI", "HealthUI", "PauseMenuManager", "WinLosePanel",
            "PlayButton", "ExitButton"
        };

        foreach (string scriptName in requiredScripts)
        {
            bool found = FindScript(scriptName);
            AddResult(scriptName + ".cs", 
                found ? "Script exists" : "Script missing!", 
                found ? ValidationStatus.Pass : ValidationStatus.Fail);
        }
    }

    void ValidateScenes()
    {
        AddHeader("🎬 SCENE VALIDATION");

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        
        AddResult("Build Settings - Scenes", 
            $"{scenes.Length} scene(s) in build settings", 
            scenes.Length > 0 ? ValidationStatus.Pass : ValidationStatus.Warning);

        bool hasMainMenu = false;
        bool hasLevel1 = false;

        foreach (var scene in scenes)
        {
            if (scene.path.Contains("MainMenu") || scene.path.Contains("MainMenuLama"))
                hasMainMenu = true;
            if (scene.path.Contains("Level1"))
                hasLevel1 = true;
        }

        AddResult("Main Menu Scene", 
            hasMainMenu ? "Found in build settings" : "Not found in build settings", 
            hasMainMenu ? ValidationStatus.Pass : ValidationStatus.Fail);

        AddResult("Level1 Scene", 
            hasLevel1 ? "Found in build settings" : "Not found - create this scene!", 
            hasLevel1 ? ValidationStatus.Pass : ValidationStatus.Warning);
    }

    void ValidateInputSystem()
    {
        AddHeader("🎮 INPUT SYSTEM VALIDATION");

        string inputAssetPath = "Assets/UTS/MidTermGameDev/Assets/InputSystem_Actions.inputactions";
        var inputAsset = AssetDatabase.LoadAssetAtPath<Object>(inputAssetPath);

        AddResult("Input Actions Asset", 
            inputAsset != null ? "Found" : "Not found at " + inputAssetPath, 
            inputAsset != null ? ValidationStatus.Pass : ValidationStatus.Warning);

        bool inputSystemPackage = false;
        var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
        foreach (var pkg in packages)
        {
            if (pkg.name == "com.unity.inputsystem")
            {
                inputSystemPackage = true;
                break;
            }
        }

        AddResult("Input System Package", 
            inputSystemPackage ? "Installed" : "Not installed", 
            inputSystemPackage ? ValidationStatus.Pass : ValidationStatus.Fail);
    }

    void ValidateTagsAndLayers()
    {
        AddHeader("🏷️ TAGS & LAYERS VALIDATION");

        string[] requiredTags = new string[] { "Player", "Finish", "GameController" };
        foreach (string tag in requiredTags)
        {
            bool exists = TagExists(tag);
            AddResult($"Tag: {tag}", 
                exists ? "Exists" : "Missing - add this tag!", 
                exists ? ValidationStatus.Pass : ValidationStatus.Warning);
        }

        string[] requiredLayers = new string[] { "Level", "SpawnArea" };
        foreach (string layer in requiredLayers)
        {
            bool exists = LayerExists(layer);
            AddResult($"Layer: {layer}", 
                exists ? "Exists" : "Missing - add this layer!", 
                exists ? ValidationStatus.Pass : ValidationStatus.Warning);
        }
    }

    void ValidateCurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        AddHeader($"🎯 CURRENT SCENE: {activeScene.name}");

        if (activeScene.name.Contains("MainMenu") || activeScene.name.Contains("MainMenuLama"))
        {
            ValidateMainMenuScene();
        }
        else if (activeScene.name.Contains("Level"))
        {
            ValidateGameplayScene();
        }
        else
        {
            AddResult("Scene Type", "Unknown scene type", ValidationStatus.Warning);
        }
    }

    void ValidateMainMenuScene()
    {
        SceneLoader sceneLoader = FindFirstObjectByType<SceneLoader>();
        AddResult("SceneLoader", 
            sceneLoader != null ? "Found" : "Missing - create SceneLoader GameObject", 
            sceneLoader != null ? ValidationStatus.Pass : ValidationStatus.Warning);

        PlayButton playButton = FindFirstObjectByType<PlayButton>();
        AddResult("PlayButton", 
            playButton != null ? "Found" : "Missing - check UI buttons", 
            playButton != null ? ValidationStatus.Pass : ValidationStatus.Warning);

        ExitButton exitButton = FindFirstObjectByType<ExitButton>();
        AddResult("ExitButton", 
            exitButton != null ? "Found" : "Missing - check UI buttons", 
            exitButton != null ? ValidationStatus.Pass : ValidationStatus.Warning);
    }

    void ValidateGameplayScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AddResult("Player GameObject", 
            player != null ? $"Found: {player.name}" : "No GameObject with 'Player' tag", 
            player != null ? ValidationStatus.Pass : ValidationStatus.Warning);

        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            AddResult("PlayerController", 
                pc != null ? "Attached to player" : "Missing on player", 
                pc != null ? ValidationStatus.Pass : ValidationStatus.Warning);

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            AddResult("PlayerHealth", 
                ph != null ? "Attached to player" : "Missing on player", 
                ph != null ? ValidationStatus.Pass : ValidationStatus.Warning);
        }

        GunBotAI[] enemies = FindObjectsByType<GunBotAI>(FindObjectsSortMode.None);
        AddResult("GunBot Enemies", 
            enemies.Length > 0 ? $"Found {enemies.Length} enemy(ies)" : "No enemies in scene", 
            enemies.Length > 0 ? ValidationStatus.Pass : ValidationStatus.Warning);

        NavMeshSurface[] navMeshSurfaces = FindObjectsByType<NavMeshSurface>(FindObjectsSortMode.None);
        bool hasNavMesh = navMeshSurfaces.Length > 0 || NavMesh.CalculateTriangulation().vertices.Length > 0;
        AddResult("NavMesh", 
            hasNavMesh ? "NavMesh data found" : "No NavMesh - bake NavMesh!", 
            hasNavMesh ? ValidationStatus.Pass : ValidationStatus.Warning);

        GameManager gm = FindFirstObjectByType<GameManager>();
        AddResult("GameManager", 
            gm != null ? "Found" : "Missing - create GameManager", 
            gm != null ? ValidationStatus.Pass : ValidationStatus.Warning);

        DiamondSpawner ds = FindFirstObjectByType<DiamondSpawner>();
        AddResult("DiamondSpawner", 
            ds != null ? "Found" : "Missing - create DiamondSpawner", 
            ds != null ? ValidationStatus.Pass : ValidationStatus.Warning);

        Camera mainCam = Camera.main;
        AddResult("Main Camera", 
            mainCam != null ? "Found" : "No camera with MainCamera tag", 
            mainCam != null ? ValidationStatus.Pass : ValidationStatus.Warning);
    }

    void DrawValidationResult(ValidationResult result)
    {
        if (result.isHeader)
        {
            GUILayout.Space(10);
            GUILayout.Label(result.category, EditorStyles.boldLabel);
            return;
        }

        Color originalColor = GUI.backgroundColor;

        switch (result.status)
        {
            case ValidationStatus.Pass:
                GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
                break;
            case ValidationStatus.Fail:
                GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);
                break;
            case ValidationStatus.Warning:
                GUI.backgroundColor = new Color(1f, 1f, 0.6f);
                break;
        }

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        
        string icon = result.status == ValidationStatus.Pass ? "✅" : 
                     result.status == ValidationStatus.Fail ? "❌" : "⚠️";
        
        GUILayout.Label(icon, GUILayout.Width(30));
        GUILayout.Label(result.category, GUILayout.Width(200));
        GUILayout.Label(result.message);
        
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = originalColor;
    }

    void DrawSummary()
    {
        int passed = 0, failed = 0, warnings = 0;

        foreach (var result in results)
        {
            if (result.isHeader) continue;
            
            switch (result.status)
            {
                case ValidationStatus.Pass: passed++; break;
                case ValidationStatus.Fail: failed++; break;
                case ValidationStatus.Warning: warnings++; break;
            }
        }

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("📊 SUMMARY", EditorStyles.boldLabel);
        GUILayout.Label($"✅ Passed: {passed}");
        GUILayout.Label($"❌ Failed: {failed}");
        GUILayout.Label($"⚠️ Warnings: {warnings}");

        GUILayout.Space(5);

        if (failed == 0 && warnings == 0)
        {
            GUILayout.Label("🎉 PERFECT! Everything is ready!", EditorStyles.boldLabel);
        }
        else if (failed == 0)
        {
            GUILayout.Label("👍 Good! Only minor warnings.", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("⚠️ Some issues need attention.", EditorStyles.boldLabel);
        }

        EditorGUILayout.EndVertical();
    }

    void AddHeader(string title)
    {
        results.Add(new ValidationResult { isHeader = true, category = title });
    }

    void AddResult(string category, string message, ValidationStatus status)
    {
        results.Add(new ValidationResult 
        { 
            category = category, 
            message = message, 
            status = status 
        });
    }

    bool FindScript(string scriptName)
    {
        string[] guids = AssetDatabase.FindAssets($"t:Script {scriptName}");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains(scriptName + ".cs"))
                return true;
        }
        return false;
    }

    bool TagExists(string tag)
    {
        try
        {
            GameObject.FindGameObjectWithTag(tag);
            return true;
        }
        catch
        {
            return false;
        }
    }

    bool LayerExists(string layerName)
    {
        return LayerMask.NameToLayer(layerName) != -1;
    }

    void OpenSetupGuide()
    {
        string path = "Assets/UTS/MidTermGameDev/START_HERE.md";
        Object readme = AssetDatabase.LoadAssetAtPath<Object>(path);
        if (readme != null)
        {
            Selection.activeObject = readme;
            EditorGUIUtility.PingObject(readme);
        }
    }

    enum ValidationStatus
    {
        Pass,
        Fail,
        Warning
    }

    class ValidationResult
    {
        public bool isHeader;
        public string category;
        public string message;
        public ValidationStatus status;
    }
}
