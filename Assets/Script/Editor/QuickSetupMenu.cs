using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class QuickSetupMenu : EditorWindow
{
    [MenuItem("MidTerm Game/Quick Setup Menu")]
    static void ShowWindow()
    {
        var window = GetWindow<QuickSetupMenu>("Quick Setup");
        window.minSize = new Vector2(400, 600);
    }

    Vector2 scrollPos;

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("🎮 MIDTERM GAME QUICK SETUP", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("AUTOMATED SETUP TOOLS - Click buttons to auto-configure!", MessageType.Info);
        GUILayout.Space(10);

        DrawSection("📋 SCENE SETUP", () =>
        {
            if (GUILayout.Button("Create Level Config GameObject", GUILayout.Height(30)))
            {
                CreateLevelConfig();
            }
            if (GUILayout.Button("Create Game Manager GameObject", GUILayout.Height(30)))
            {
                CreateGameManager();
            }
            if (GUILayout.Button("Create Pause Menu Manager", GUILayout.Height(30)))
            {
                CreatePauseMenuManager();
            }
        });

        DrawSection("🧟 PLAYER SETUP", () =>
        {
            if (GUILayout.Button("Setup Selected as Player (Zombie)", GUILayout.Height(30)))
            {
                SetupSelectedAsPlayer();
            }
            EditorGUILayout.HelpBox("Select Zombie model in hierarchy first", MessageType.None);
        });

        DrawSection("🤖 ENEMY SETUP", () =>
        {
            if (GUILayout.Button("Setup Selected as GunBot", GUILayout.Height(30)))
            {
                SetupSelectedAsGunBot();
            }
            if (GUILayout.Button("Create Patrol Points System", GUILayout.Height(30)))
            {
                CreatePatrolPoints();
            }
            EditorGUILayout.HelpBox("Select GunBot model, scale it to 0.01 first", MessageType.None);
        });

        DrawSection("💎 DIAMOND SYSTEM", () =>
        {
            if (GUILayout.Button("Create Diamond Spawner", GUILayout.Height(30)))
            {
                CreateDiamondSpawner();
            }
            if (GUILayout.Button("Setup Selected as Spawn Area", GUILayout.Height(30)))
            {
                SetupSpawnArea();
            }
            if (GUILayout.Button("Fix Diamond Prefab", GUILayout.Height(30)))
            {
                FixDiamondPrefab();
            }
        });

        DrawSection("📸 CAMERA SETUP", () =>
        {
            if (GUILayout.Button("Create Third Person Camera", GUILayout.Height(30)))
            {
                CreateThirdPersonCamera();
            }
            if (GUILayout.Button("Setup Main Camera for Player", GUILayout.Height(30)))
            {
                SetupMainCameraForPlayer();
            }
        });

        DrawSection("🎨 UI SETUP", () =>
        {
            if (GUILayout.Button("Create Game UI", GUILayout.Height(30)))
            {
                CreateGameUI();
            }
            if (GUILayout.Button("Create Health UI", GUILayout.Height(30)))
            {
                CreateHealthUI();
            }
        });

        GUILayout.Space(20);
        
        if (GUILayout.Button("📖 OPEN SETUP GUIDE", GUILayout.Height(50)))
        {
            OpenSetupGuide();
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawSection(string title, System.Action content)
    {
        GUILayout.Space(10);
        GUILayout.Label(title, EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        content();
        EditorGUILayout.EndVertical();
    }

    void CreateLevelConfig()
    {
        GameObject go = new GameObject("LevelConfig");
        go.AddComponent<LevelConfig>();
        Selection.activeGameObject = go;
        EditorGUIUtility.PingObject(go);
        Debug.Log("✅ Created LevelConfig GameObject");
    }

    void CreateGameManager()
    {
        GameObject go = GameObject.Find("GameManager");
        if (go == null)
        {
            go = new GameObject("GameManager");
        }

        if (go.GetComponent<GameManager>() == null)
        {
            go.AddComponent<GameManager>();
        }

        Selection.activeGameObject = go;
        EditorGUIUtility.PingObject(go);
        Debug.Log("✅ Created GameManager GameObject");
    }

    void CreatePauseMenuManager()
    {
        GameObject go = new GameObject("PauseMenuManager");
        go.AddComponent<PauseMenuManager>();
        Selection.activeGameObject = go;
        EditorGUIUtility.PingObject(go);
        Debug.Log("✅ Created PauseMenuManager - Remember to assign Pause Panel!");
    }

    void SetupSelectedAsPlayer()
    {
        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("No Selection", "Please select Zombie model in hierarchy first", "OK");
            return;
        }

        GameObject zombie = Selection.activeGameObject;
        zombie.tag = "Player";

        Rigidbody rb = zombie.GetComponent<Rigidbody>();
        if (rb == null) rb = zombie.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        CapsuleCollider col = zombie.GetComponent<CapsuleCollider>();
        if (col == null)
        {
            col = zombie.AddComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.5f;
        }

        if (zombie.GetComponent<PlayerController>() == null)
        {
            zombie.AddComponent<PlayerController>();
        }

        if (zombie.GetComponent<PlayerHealth>() == null)
        {
            PlayerHealth ph = zombie.AddComponent<PlayerHealth>();
            ph.maxHealth = 100;
        }

        NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        Debug.Log($"✅ Setup {zombie.name} as Player!");
    }

    void SetupSelectedAsGunBot()
    {
        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("No Selection", "Please select GunBot model in hierarchy first", "OK");
            return;
        }

        GameObject gunBot = Selection.activeGameObject;
        
        if (gunBot.transform.localScale.x > 0.1f)
        {
            gunBot.transform.localScale = Vector3.one * 0.01f;
        }

        NavMeshAgent agent = gunBot.GetComponent<NavMeshAgent>();
        if (agent == null) agent = gunBot.AddComponent<NavMeshAgent>();

        CapsuleCollider col = gunBot.GetComponent<CapsuleCollider>();
        if (col == null) col = gunBot.AddComponent<CapsuleCollider>();

        if (gunBot.GetComponent<GunBotAI>() == null)
        {
            GunBotAI ai = gunBot.AddComponent<GunBotAI>();
            ai.detectionRadius = 500f;
            ai.losePlayerRadius = 600f;
            ai.attackRange = 80f;
            ai.patrolSpeed = 150f;
            ai.chaseSpeed = 300f;
            ai.attackDamage = 10;
        }

        Debug.Log($"✅ Setup {gunBot.name} as GunBot Enemy!");
    }

    void CreatePatrolPoints()
    {
        GameObject points = new GameObject("Points");
        
        for (int i = 1; i <= 4; i++)
        {
            GameObject point = new GameObject($"Point{i}");
            point.transform.parent = points.transform;
            point.transform.position = new Vector3(i * 10f, 0, i * 10f);
        }

        Selection.activeGameObject = points;
        EditorGUIUtility.PingObject(points);
        Debug.Log("✅ Created Patrol Points - Position them around your map!");
    }

    void CreateDiamondSpawner()
    {
        GameObject spawner = new GameObject("DiamondSpawner");
        DiamondSpawner ds = spawner.AddComponent<DiamondSpawner>();
        ds.totalDiamonds = 10;
        ds.autoSpawnOnStart = true;
        ds.enableRespawn = false;

        Selection.activeGameObject = spawner;
        EditorGUIUtility.PingObject(spawner);
        Debug.Log("✅ Created DiamondSpawner - Assign Diamond Prefab and Spawn Areas!");
    }

    void SetupSpawnArea()
    {
        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("No Selection", "Select a GameObject to convert to Spawn Area", "OK");
            return;
        }

        GameObject obj = Selection.activeGameObject;
        BoxCollider box = obj.GetComponent<BoxCollider>();
        if (box == null)
        {
            box = obj.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        obj.layer = LayerMask.NameToLayer("SpawnArea");
        
        Debug.Log($"✅ Setup {obj.name} as Spawn Area - Add to DiamondSpawner's Spawn Areas list!");
    }

    void FixDiamondPrefab()
    {
        string path = "Assets/UTS/MidTermGameDev/Assets/Asset/Diamond.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        
        if (prefab == null)
        {
            Debug.LogWarning("Diamond.prefab not found at " + path);
            return;
        }

        GameObject instance = PrefabUtility.LoadPrefabContents(path);
        
        if (instance.GetComponent<Diamond>() == null)
        {
            instance.AddComponent<Diamond>();
        }

        BoxCollider col = instance.GetComponent<BoxCollider>();
        if (col != null) col.isTrigger = true;

        PrefabUtility.SaveAsPrefabAsset(instance, path);
        PrefabUtility.UnloadPrefabContents(instance);
        
        Debug.Log("✅ Fixed Diamond Prefab!");
    }

    void CreateThirdPersonCamera()
    {
        GameObject camObj = new GameObject("ThirdPersonCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.tag = "MainCamera";
        
        ThirdPersonCamera tpc = camObj.AddComponent<ThirdPersonCamera>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            tpc.target = player.transform;
        }

        camObj.AddComponent<AudioListener>();
        
        Selection.activeGameObject = camObj;
        EditorGUIUtility.PingObject(camObj);
        Debug.Log("✅ Created Third Person Camera!");
    }

    void SetupMainCameraForPlayer()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogWarning("No Main Camera found!");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No Player found!");
            return;
        }

        if (mainCam.GetComponent<ThirdPersonCamera>() == null)
        {
            ThirdPersonCamera tpc = mainCam.gameObject.AddComponent<ThirdPersonCamera>();
            tpc.target = player.transform;
        }

        Debug.Log("✅ Setup Main Camera for Player!");
    }

    void CreateGameUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Create a Canvas first!");
            return;
        }

        GameObject uiObj = new GameObject("GameUI");
        uiObj.transform.SetParent(canvas.transform, false);
        uiObj.AddComponent<GameUI>();

        Debug.Log("✅ Created GameUI - Add text elements and link them!");
    }

    void CreateHealthUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Create a Canvas first!");
            return;
        }

        GameObject healthObj = new GameObject("HealthUI");
        healthObj.transform.SetParent(canvas.transform, false);
        healthObj.AddComponent<HealthUI>();

        Debug.Log("✅ Created HealthUI - Add Slider and configure!");
    }

    void OpenSetupGuide()
    {
        string path = "Assets/UTS/MidTermGameDev/README_SETUP.md";
        Object readme = AssetDatabase.LoadAssetAtPath<Object>(path);
        if (readme != null)
        {
            Selection.activeObject = readme;
            EditorGUIUtility.PingObject(readme);
        }
        else
        {
            Debug.Log("Setup guide at: " + path);
        }
    }
}
