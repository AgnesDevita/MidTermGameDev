using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class AutoSetupLevel1 : EditorWindow
{
    [MenuItem("MidTerm Game/Auto Setup Level 1")]
    static void ShowWindow()
    {
        GetWindow<AutoSetupLevel1>("Setup Level 1");
    }

    void OnGUI()
    {
        GUILayout.Label("Level 1 Auto Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("1. Setup Zombie Player", GUILayout.Height(40)))
        {
            SetupZombiePlayer();
        }

        if (GUILayout.Button("2. Setup GunBot Enemy", GUILayout.Height(40)))
        {
            SetupGunBot();
        }

        if (GUILayout.Button("3. Setup Game Manager", GUILayout.Height(40)))
        {
            SetupGameManager();
        }

        if (GUILayout.Button("4. Setup Diamond Spawner", GUILayout.Height(40)))
        {
            SetupDiamondSpawner();
        }

        if (GUILayout.Button("5. Setup Pause Menu", GUILayout.Height(40)))
        {
            SetupPauseMenu();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("⭐ SETUP ALL ⭐", GUILayout.Height(60)))
        {
            SetupAll();
        }

        GUILayout.Space(10);
        EditorGUILayout.HelpBox(
            "Make sure you have:\n" +
            "- Level1 scene created\n" +
            "- Room model in scene\n" +
            "- NavMesh baked\n" +
            "- Zombie and GunBot models imported",
            MessageType.Info);
    }

    void SetupAll()
    {
        SetupGameManager();
        SetupZombiePlayer();
        SetupGunBot();
        SetupDiamondSpawner();
        SetupPauseMenu();
        Debug.Log("✅ All setup complete!");
    }

    void SetupZombiePlayer()
    {
        GameObject zombie = GameObject.FindGameObjectWithTag("Player");
        if (zombie == null)
        {
            zombie = GameObject.Find("Zombie");
            if (zombie == null)
            {
                Debug.LogWarning("Zombie not found! Add Zombie model to scene first.");
                return;
            }
        }

        if (!zombie.CompareTag("Player"))
        {
            zombie.tag = "Player";
        }

        Rigidbody rb = zombie.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = zombie.AddComponent<Rigidbody>();
        }
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
        if (agent != null)
        {
            agent.enabled = false;
        }

        Debug.Log("✅ Zombie Player setup complete!");
    }

    void SetupGunBot()
    {
        GameObject gunBot = GameObject.Find("GunBot");
        if (gunBot == null)
        {
            gunBot = GameObject.Find("gun-bot_with_walk_and_idle_animation");
            if (gunBot == null)
            {
                Debug.LogWarning("GunBot not found! Add GunBot model to scene first.");
                return;
            }
        }

        if (gunBot.transform.localScale.x > 0.1f)
        {
            gunBot.transform.localScale = Vector3.one * 0.01f;
        }

        NavMeshAgent agent = gunBot.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gunBot.AddComponent<NavMeshAgent>();
        }

        CapsuleCollider col = gunBot.GetComponent<CapsuleCollider>();
        if (col == null)
        {
            col = gunBot.AddComponent<CapsuleCollider>();
        }

        if (gunBot.GetComponent<GunBotAI>() == null)
        {
            GunBotAI ai = gunBot.AddComponent<GunBotAI>();
            ai.detectionRadius = 500f;
            ai.losePlayerRadius = 600f;
            ai.attackRange = 80f;
            ai.patrolSpeed = 150f;
            ai.chaseSpeed = 300f;
            ai.attackDamage = 10;
            ai.attackCooldown = 1.5f;
        }

        Debug.Log("✅ GunBot setup complete!");
    }

    void SetupGameManager()
    {
        GameObject gmObj = GameObject.Find("GameManager");
        if (gmObj == null)
        {
            gmObj = new GameObject("GameManager");
        }

        if (gmObj.GetComponent<GameManager>() == null)
        {
            GameManager gm = gmObj.AddComponent<GameManager>();
            gm.totalDiamonds = 10;
            gm.autoWinOnComplete = true;
        }

        if (gmObj.GetComponent<LevelConfig>() == null)
        {
            LevelConfig lc = gmObj.AddComponent<LevelConfig>();
            lc.levelNumber = 1;
            lc.diamondCount = 10;
        }

        Debug.Log("✅ GameManager setup complete!");
    }

    void SetupDiamondSpawner()
    {
        GameObject spawnerObj = GameObject.Find("DiamondSpawner");
        if (spawnerObj == null)
        {
            spawnerObj = new GameObject("DiamondSpawner");
        }

        if (spawnerObj.GetComponent<DiamondSpawner>() == null)
        {
            DiamondSpawner ds = spawnerObj.AddComponent<DiamondSpawner>();
            ds.totalDiamonds = 10;
            ds.autoSpawnOnStart = true;
            ds.enableRespawn = false;
            ds.minSpacing = 2f;
            Debug.Log("⚠️ Remember to assign Diamond Prefab and Spawn Areas to DiamondSpawner!");
        }

        Debug.Log("✅ DiamondSpawner created! Configure in Inspector.");
    }

    void SetupPauseMenu()
    {
        GameObject pauseObj = GameObject.Find("PauseMenuManager");
        if (pauseObj == null)
        {
            pauseObj = new GameObject("PauseMenuManager");
        }

        if (pauseObj.GetComponent<PauseMenuManager>() == null)
        {
            PauseMenuManager pm = pauseObj.AddComponent<PauseMenuManager>();
            pm.lockCursorInGameplay = true;
            Debug.Log("⚠️ Remember to assign Pause Panel to PauseMenuManager!");
        }

        Debug.Log("✅ PauseMenuManager setup complete!");
    }
}
