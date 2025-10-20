using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[InitializeOnLoad]
public class AutoSetupDiamondSystem
{
    private const string SETUP_KEY = "DiamondSystem_AutoSetup_Done_v8";
    
    static AutoSetupDiamondSystem()
    {
        EditorApplication.delayCall += OnEditorLoaded;
    }
    
    static void OnEditorLoaded()
    {
        string currentScene = SceneManager.GetActiveScene().path;
        
        if (!currentScene.Contains("Level1"))
        {
            return;
        }
        
        bool alreadySetup = EditorPrefs.GetBool(SETUP_KEY, false);
        
        if (alreadySetup)
        {
            return;
        }
        
        Debug.Log("🚀 AUTO-SETUP: Starting Diamond System setup...");
        
        SetupDiamondSystem();
        
        EditorPrefs.SetBool(SETUP_KEY, true);
        
        Debug.Log("✅ AUTO-SETUP COMPLETE! Diamond system ready to play!");
        Debug.Log("⚠️ IMPORTANT: Make sure Zombie GameObject has tag 'Player'!");
    }
    
    [MenuItem("Tools/Diamond System/Force Auto-Setup Now")]
    static void ForceSetup()
    {
        EditorPrefs.SetBool(SETUP_KEY, false);
        SetupDiamondSystem();
        EditorPrefs.SetBool(SETUP_KEY, true);
    }
    
    [MenuItem("Tools/Diamond System/Reset Setup (Run Again)")]
    static void ResetSetup()
    {
        EditorPrefs.DeleteKey(SETUP_KEY);
        Debug.Log("Setup flag cleared. Reload scene or use Force Auto-Setup.");
    }
    
    static void SetupDiamondSystem()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        if (!activeScene.IsValid() || !activeScene.isLoaded)
        {
            Debug.LogError("No valid scene loaded!");
            return;
        }
        
        Step1_AddDiamondScripts();
        Step2_SetupDiamondSpawner();
        Step3_CreateLevelConfig();
        Step4_CreateGameManager();
        Step5_CreateGameUI();
        Step6_CreateWinPanel();
        Step7_CreateGameOverPanel();
        Step8_CreateHealthUI();
        Step9_AddPlayerHealth();
        Step10_LinkReferences();
        Step11_VerifyZombieTag();
        
        EditorSceneManager.MarkSceneDirty(activeScene);
        
        Debug.Log("💾 Scene marked as dirty. Remember to SAVE scene! (Ctrl+S)");
    }
    
    static void Step1_AddDiamondScripts()
    {
        Debug.Log("Step 1: Adding Diamond scripts and enlarging diamonds...");
        
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int added = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Diamond") && obj.GetComponent<Diamond>() == null)
            {
                Diamond diamond = obj.AddComponent<Diamond>();
                diamond.autoScaleMultiplier = 3f;
                diamond.enableRotation = false;
                diamond.rotationSpeed = 0f;
                diamond.enableFloating = false;
                diamond.floatAmplitude = 0f;
                diamond.floatSpeed = 0f;
                
                BoxCollider collider = obj.GetComponent<BoxCollider>();
                if (collider != null)
                {
                    collider.isTrigger = true;
                }
                
                if (obj.transform.localScale.magnitude < 5f)
                {
                    obj.transform.localScale *= 3f;
                }
                
                added++;
            }
        }
        
        Debug.Log($"  ✅ Added Diamond script to {added} objects - STATIC ITEMS (no animation)");
    }
    
    static void Step2_SetupDiamondSpawner()
    {
        Debug.Log("Step 2: Setting up DiamondSpawner...");
        
        DiamondSpawner spawner = Object.FindFirstObjectByType<DiamondSpawner>();
        
        if (spawner == null)
        {
            Debug.LogWarning("  ⚠️ DiamondSpawner not found in scene!");
            return;
        }
        
        spawner.autoSpawnOnStart = true;
        spawner.enableRespawn = true;
        spawner.respawnDelay = 5f;
        spawner.totalDiamonds = 15;
        spawner.minSpacing = 2f;
        spawner.raycastHeight = 10f;
        spawner.heightOffset = 0.5f;
        spawner.useNavMeshCheck = true;
        spawner.navMeshMaxDistance = 2f;
        spawner.maxAttemptsPerDiamond = 100;
        
        int layerLevel = LayerMask.NameToLayer("Level");
        if (layerLevel >= 0)
        {
            spawner.groundMask = 1 << layerLevel;
            Debug.Log($"  ✅ Ground layer set to 'Level' (layer {layerLevel})");
        }
        else
        {
            spawner.groundMask = ~0;
            Debug.LogWarning("  ⚠️ 'Level' layer not found, using all layers for ground");
        }
        
        spawner.obstacleMask = 0;
        
        if (spawner.spawnAreas == null || spawner.spawnAreas.Count == 0)
        {
            GameObject plane = GameObject.Find("Plane");
            if (plane != null)
            {
                BoxCollider planeCollider = plane.GetComponent<BoxCollider>();
                if (planeCollider == null)
                {
                    planeCollider = plane.AddComponent<BoxCollider>();
                    planeCollider.isTrigger = false;
                }
                spawner.spawnAreas = new List<BoxCollider> { planeCollider };
                Debug.Log("  ✅ Added Plane as spawn area");
            }
            else
            {
                Debug.LogWarning("  ⚠️ No spawn areas configured and Plane not found!");
            }
        }
        
        Debug.Log("  ✅ DiamondSpawner configured for random NavMesh-based spawning with height offset");
    }
    
    static void Step3_CreateLevelConfig()
    {
        Debug.Log("Step 3: Creating LevelConfig...");
        
        LevelConfig existing = Object.FindFirstObjectByType<LevelConfig>();
        if (existing != null)
        {
            Debug.Log("  ⚠️ LevelConfig already exists");
            return;
        }
        
        GameObject levelConfigObj = new GameObject("_LevelConfig");
        LevelConfig config = levelConfigObj.AddComponent<LevelConfig>();
        
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("Level1"))
        {
            config.levelNumber = 1;
            config.diamondCount = 10;
            config.gunBotSpeedMultiplier = 1f;
            config.gunBotDetectionMultiplier = 1f;
            config.gunBotDamageMultiplier = 1f;
            config.gunBotAttackSpeedMultiplier = 1f;
            Debug.Log("  ✅ LevelConfig created for LEVEL 1: 10 diamonds, GunBot 1x");
        }
        else if (sceneName.Contains("Level2"))
        {
            config.levelNumber = 2;
            config.diamondCount = 20;
            config.gunBotSpeedMultiplier = 2f;
            config.gunBotDetectionMultiplier = 2f;
            config.gunBotDamageMultiplier = 2f;
            config.gunBotAttackSpeedMultiplier = 2f;
            Debug.Log("  ✅ LevelConfig created for LEVEL 2: 20 diamonds, GunBot 2x AGGRESSIVE!");
        }
        else
        {
            config.levelNumber = 1;
            config.diamondCount = 10;
            Debug.Log("  ✅ LevelConfig created with default settings");
        }
    }
    
    static void Step4_CreateGameManager()
    {
        Debug.Log("Step 4: Creating GameManager...");
        
        GameManager existing = Object.FindFirstObjectByType<GameManager>();
        if (existing != null)
        {
            Debug.Log("  ⚠️ GameManager already exists, skipping");
            return;
        }
        
        GameObject gmObj = new GameObject("_GameManager");
        gmObj.AddComponent<GameManager>();
        
        Debug.Log("  ✅ GameManager created");
    }
    
    static void Step5_CreateGameUI()
    {
        Debug.Log("Step 5: Creating Game UI...");
        
        Canvas existingCanvas = Object.FindFirstObjectByType<Canvas>();
        GameObject uiCanvas = null;
        
        if (existingCanvas != null && existingCanvas.name.Contains("GameUI"))
        {
            uiCanvas = existingCanvas.gameObject;
            Debug.Log("  ⚠️ GameUI Canvas already exists");
        }
        else
        {
            uiCanvas = new GameObject("GameUI_Canvas");
            Canvas canvas = uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = uiCanvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            uiCanvas.AddComponent<GraphicRaycaster>();
            
            Debug.Log("  ✅ Canvas created");
        }
        
        TextMeshProUGUI levelText = CreateOrGetUIText(uiCanvas, "LevelText", 
            new Vector2(20, -20), "Level 1", 36, new Color(0.3f, 1f, 0.3f), TextAlignmentOptions.TopLeft);
        
        TextMeshProUGUI scoreText = CreateOrGetUIText(uiCanvas, "ScoreText", 
            new Vector2(20, -70), "Score: 0", 32, Color.white, TextAlignmentOptions.TopLeft);
        
        TextMeshProUGUI diamondText = CreateOrGetUIText(uiCanvas, "DiamondCountText", 
            new Vector2(20, -115), "Diamonds: 0/10", 28, Color.yellow, TextAlignmentOptions.TopLeft);
        
        GameUI gameUI = uiCanvas.GetComponent<GameUI>();
        if (gameUI == null)
        {
            gameUI = uiCanvas.AddComponent<GameUI>();
        }
        
        gameUI.levelText = levelText;
        gameUI.scoreText = scoreText;
        gameUI.diamondCountText = diamondText;
        
        Debug.Log("  ✅ UI elements created and linked (Level + Score + Diamonds)");
    }
    
    static TextMeshProUGUI CreateOrGetUIText(GameObject parent, string name, Vector2 position, 
        string text, int fontSize, Color color, TextAlignmentOptions alignment)
    {
        Transform existing = parent.transform.Find(name);
        GameObject textObj;
        
        if (existing != null)
        {
            textObj = existing.gameObject;
        }
        else
        {
            textObj = new GameObject(name);
            textObj.transform.SetParent(parent.transform, false);
        }
        
        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
        if (tmp == null)
        {
            tmp = textObj.AddComponent<TextMeshProUGUI>();
        }
        
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(400, 50);
        
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.fontStyle = FontStyles.Bold;
        tmp.outlineWidth = 0.2f;
        tmp.outlineColor = Color.black;
        
        return tmp;
    }
    
    static void Step6_CreateWinPanel()
    {
        Debug.Log("Step 6: Creating Win Panel...");
        
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("  ⚠️ No Canvas found!");
            return;
        }
        
        Transform existing = canvas.transform.Find("WinPanel");
        if (existing != null)
        {
            Debug.Log("  ⚠️ WinPanel already exists");
            return;
        }
        
        GameObject winPanel = new GameObject("WinPanel");
        winPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform panelRect = winPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        
        Image panelBg = winPanel.AddComponent<Image>();
        panelBg.color = new Color(0, 0, 0, 0.9f);
        
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(winPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = new Vector2(0, 100);
        titleRect.sizeDelta = new Vector2(600, 100);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "🎉 VICTORY! 🎉";
        titleText.fontSize = 72;
        titleText.color = Color.yellow;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;
        titleText.outlineWidth = 0.3f;
        titleText.outlineColor = Color.black;
        
        GameObject subtitleObj = new GameObject("Subtitle");
        subtitleObj.transform.SetParent(winPanel.transform, false);
        RectTransform subtitleRect = subtitleObj.AddComponent<RectTransform>();
        subtitleRect.anchorMin = new Vector2(0.5f, 0.5f);
        subtitleRect.anchorMax = new Vector2(0.5f, 0.5f);
        subtitleRect.anchoredPosition = new Vector2(0, 0);
        subtitleRect.sizeDelta = new Vector2(500, 80);
        
        TextMeshProUGUI subtitleText = subtitleObj.AddComponent<TextMeshProUGUI>();
        subtitleText.text = "All Diamonds Collected!";
        subtitleText.fontSize = 36;
        subtitleText.color = Color.white;
        subtitleText.alignment = TextAlignmentOptions.Center;
        subtitleText.fontStyle = FontStyles.Bold;
        
        GameObject restartBtn = CreateButton(winPanel, "RestartButton", new Vector2(0, -100), "Play Again", new Color(0.2f, 0.8f, 0.2f));
        GameObject menuBtn = CreateButton(winPanel, "MenuButton", new Vector2(0, -180), "Main Menu", new Color(0.8f, 0.2f, 0.2f));
        
        UnityEngine.UI.Button restartComp = restartBtn.GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button menuComp = menuBtn.GetComponent<UnityEngine.UI.Button>();
        
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            UnityEngine.Events.UnityAction restartAction = new UnityEngine.Events.UnityAction(gm.RestartGame);
            UnityEngine.Events.UnityAction menuAction = new UnityEngine.Events.UnityAction(gm.LoadMainMenu);
            restartComp.onClick.AddListener(restartAction);
            menuComp.onClick.AddListener(menuAction);
            
            gm.winPanel = winPanel;
        }
        
        winPanel.SetActive(false);
        
        Debug.Log("  ✅ Win Panel created with buttons");
    }
    
    static void Step7_CreateGameOverPanel()
    {
        Debug.Log("Step 7: Creating Game Over Panel...");
        
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("  ⚠️ No Canvas found!");
            return;
        }
        
        Transform existing = canvas.transform.Find("GameOverPanel");
        if (existing != null)
        {
            Debug.Log("  ⚠️ GameOverPanel already exists");
            return;
        }
        
        GameObject gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform panelRect = gameOverPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        
        Image panelBg = gameOverPanel.AddComponent<Image>();
        panelBg.color = new Color(0.1f, 0, 0, 0.95f);
        
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(gameOverPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = new Vector2(0, 100);
        titleRect.sizeDelta = new Vector2(600, 100);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "💀 GAME OVER 💀";
        titleText.fontSize = 72;
        titleText.color = Color.red;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontStyle = FontStyles.Bold;
        titleText.outlineWidth = 0.3f;
        titleText.outlineColor = Color.black;
        
        GameObject subtitleObj = new GameObject("Subtitle");
        subtitleObj.transform.SetParent(gameOverPanel.transform, false);
        RectTransform subtitleRect = subtitleObj.AddComponent<RectTransform>();
        subtitleRect.anchorMin = new Vector2(0.5f, 0.5f);
        subtitleRect.anchorMax = new Vector2(0.5f, 0.5f);
        subtitleRect.anchoredPosition = new Vector2(0, 0);
        subtitleRect.sizeDelta = new Vector2(500, 80);
        
        TextMeshProUGUI subtitleText = subtitleObj.AddComponent<TextMeshProUGUI>();
        subtitleText.text = "You were defeated!";
        subtitleText.fontSize = 36;
        subtitleText.color = new Color(1f, 0.5f, 0.5f);
        subtitleText.alignment = TextAlignmentOptions.Center;
        subtitleText.fontStyle = FontStyles.Bold;
        
        GameObject restartBtn = CreateButton(gameOverPanel, "RestartButton", new Vector2(0, -100), "Try Again", new Color(0.2f, 0.8f, 0.2f));
        GameObject menuBtn = CreateButton(gameOverPanel, "MenuButton", new Vector2(0, -180), "Main Menu", new Color(0.8f, 0.2f, 0.2f));
        
        UnityEngine.UI.Button restartComp = restartBtn.GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button menuComp = menuBtn.GetComponent<UnityEngine.UI.Button>();
        
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            UnityEngine.Events.UnityAction restartAction = new UnityEngine.Events.UnityAction(gm.RestartGame);
            UnityEngine.Events.UnityAction menuAction = new UnityEngine.Events.UnityAction(gm.LoadMainMenu);
            restartComp.onClick.AddListener(restartAction);
            menuComp.onClick.AddListener(menuAction);
            
            gm.gameOverPanel = gameOverPanel;
        }
        
        gameOverPanel.SetActive(false);
        
        Debug.Log("  ✅ Game Over Panel created with buttons");
    }
    
    static GameObject CreateButton(GameObject parent, string name, Vector2 position, string text, Color color)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent.transform, false);
        
        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(300, 60);
        
        Image img = btnObj.AddComponent<Image>();
        img.color = color;
        
        UnityEngine.UI.Button btn = btnObj.AddComponent<UnityEngine.UI.Button>();
        btn.targetGraphic = img;
        
        ColorBlock colors = btn.colors;
        colors.normalColor = color;
        colors.highlightedColor = color * 1.2f;
        colors.pressedColor = color * 0.8f;
        btn.colors = colors;
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 32;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        
        return btnObj;
    }
    
    static void Step8_CreateHealthUI()
    {
        Debug.Log("Step 8: Creating Health UI...");
        
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("  ⚠️ No Canvas found!");
            return;
        }
        
        Transform existingHealthBar = canvas.transform.Find("HealthBarBG");
        if (existingHealthBar != null)
        {
            Debug.Log("  ⚠️ Health UI already exists");
            return;
        }
        
        GameObject healthBarBg = new GameObject("HealthBarBG");
        healthBarBg.transform.SetParent(canvas.transform, false);
        
        RectTransform bgRect = healthBarBg.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 1);
        bgRect.anchorMax = new Vector2(0, 1);
        bgRect.pivot = new Vector2(0, 1);
        bgRect.anchoredPosition = new Vector2(20, -170);
        bgRect.sizeDelta = new Vector2(300, 35);
        
        Image bgImage = healthBarBg.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        
        GameObject healthBarFill = new GameObject("HealthBarFill");
        healthBarFill.transform.SetParent(healthBarBg.transform, false);
        
        RectTransform fillRect = healthBarFill.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(3, 3);
        fillRect.offsetMax = new Vector2(-3, -3);
        
        Image fillImage = healthBarFill.AddComponent<Image>();
        fillImage.color = Color.green;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        
        Slider slider = healthBarBg.AddComponent<Slider>();
        slider.fillRect = fillRect;
        slider.targetGraphic = fillImage;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;
        slider.interactable = false;
        
        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.SetParent(canvas.transform, false);
        
        RectTransform textRect = healthTextObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 1);
        textRect.anchorMax = new Vector2(0, 1);
        textRect.pivot = new Vector2(0, 1);
        textRect.anchoredPosition = new Vector2(330, -170);
        textRect.sizeDelta = new Vector2(150, 35);
        
        TextMeshProUGUI healthText = healthTextObj.AddComponent<TextMeshProUGUI>();
        healthText.text = "HP: 100/100";
        healthText.fontSize = 24;
        healthText.color = Color.white;
        healthText.alignment = TextAlignmentOptions.Left;
        healthText.fontStyle = FontStyles.Bold;
        healthText.outlineWidth = 0.2f;
        healthText.outlineColor = Color.black;
        
        HealthUI healthUI = canvas.gameObject.GetComponent<HealthUI>();
        if (healthUI == null)
        {
            healthUI = canvas.gameObject.AddComponent<HealthUI>();
        }
        
        healthUI.healthBar = slider;
        healthUI.healthText = healthText;
        healthUI.healthBarFill = fillImage;
        
        Debug.Log("  ✅ Health UI created with bar and text");
    }
    
    static void Step9_AddPlayerHealth()
    {
        Debug.Log("Step 9: Adding PlayerHealth to Zombie...");
        
        GameObject zombie = GameObject.Find("Zombie");
        
        if (zombie == null)
        {
            Debug.LogWarning("  ⚠️ Zombie not found!");
            return;
        }
        
        PlayerHealth health = zombie.GetComponent<PlayerHealth>();
        if (health == null)
        {
            health = zombie.AddComponent<PlayerHealth>();
            health.maxHealth = 100;
            health.currentHealth = 100;
            health.invincibilityDuration = 1f;
            Debug.Log("  ✅ PlayerHealth added to Zombie (100 HP)");
        }
        else
        {
            Debug.Log("  ⚠️ PlayerHealth already exists on Zombie");
        }
    }
    
    static void Step10_LinkReferences()
    {
        Debug.Log("Step 10: Linking references...");
        
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        
        if (gm == null || canvas == null)
        {
            Debug.LogWarning("  ⚠️ Cannot link references - missing GameManager or Canvas");
            return;
        }
        
        Transform scoreTransform = canvas.transform.Find("ScoreText");
        Transform diamondTransform = canvas.transform.Find("DiamondCountText");
        
        if (scoreTransform != null)
        {
            gm.scoreText = scoreTransform.GetComponent<TextMeshProUGUI>();
        }
        
        if (diamondTransform != null)
        {
            gm.diamondCountText = diamondTransform.GetComponent<TextMeshProUGUI>();
        }
        
        Debug.Log("  ✅ References linked");
    }
    
    static void Step11_VerifyZombieTag()
    {
        Debug.Log("Step 11: Verifying Zombie tag...");
        
        GameObject zombie = GameObject.Find("Zombie");
        
        if (zombie == null)
        {
            Debug.LogWarning("  ⚠️ Zombie GameObject not found in scene!");
            return;
        }
        
        if (zombie.tag != "Player")
        {
            zombie.tag = "Player";
            Debug.Log("  ✅ Zombie tag set to 'Player'");
        }
        else
        {
            Debug.Log("  ✅ Zombie already has 'Player' tag");
        }
    }
}