using UnityEngine;

public class SetupChecklist : MonoBehaviour
{
    [Header("📋 SETUP CHECKLIST")]
    [Space(10)]
    
    [Header("SCENE SETUP")]
    public bool level1SceneCreated = false;
    public bool roomModelAdded = false;
    public bool navMeshBaked = false;
    public bool lightingSetup = false;
    
    [Space(10)]
    [Header("PLAYER SETUP")]
    public bool zombieAdded = false;
    public bool playerTagSet = false;
    public bool playerControllerAdded = false;
    public bool playerHealthAdded = false;
    public bool cameraSetup = false;
    
    [Space(10)]
    [Header("ENEMY SETUP")]
    public bool gunBotAdded = false;
    public bool gunBotScaled = false;
    public bool gunBotAIAdded = false;
    public bool patrolPointsCreated = false;
    
    [Space(10)]
    [Header("DIAMOND SYSTEM")]
    public bool diamondPrefabCreated = false;
    public bool diamondSpawnerCreated = false;
    public bool spawnAreasCreated = false;
    public bool diamondsSpawning = false;
    
    [Space(10)]
    [Header("GAME MANAGER")]
    public bool gameManagerCreated = false;
    public bool levelConfigAdded = false;
    public bool totalDiamondsSet = false;
    
    [Space(10)]
    [Header("UI SETUP")]
    public bool canvasCreated = false;
    public bool gameUICreated = false;
    public bool healthUICreated = false;
    public bool winPanelCreated = false;
    public bool gameOverPanelCreated = false;
    public bool pauseMenuCreated = false;
    
    [Space(10)]
    [Header("BUILD SETTINGS")]
    public bool mainMenuInBuild = false;
    public bool level1InBuild = false;
    public bool inputSystemConfigured = false;
    
    [Space(10)]
    [Header("TESTING")]
    public bool playerCanMove = false;
    public bool gunBotPatrols = false;
    public bool diamondsCollect = false;
    public bool healthDecreases = false;
    public bool winConditionWorks = false;
    public bool pauseMenuWorks = false;

    [ContextMenu("Show Progress")]
    void ShowProgress()
    {
        int total = 0;
        int completed = 0;

        total += 4;
        completed += (level1SceneCreated ? 1 : 0) + (roomModelAdded ? 1 : 0) + 
                     (navMeshBaked ? 1 : 0) + (lightingSetup ? 1 : 0);

        total += 5;
        completed += (zombieAdded ? 1 : 0) + (playerTagSet ? 1 : 0) + 
                     (playerControllerAdded ? 1 : 0) + (playerHealthAdded ? 1 : 0) + 
                     (cameraSetup ? 1 : 0);

        total += 4;
        completed += (gunBotAdded ? 1 : 0) + (gunBotScaled ? 1 : 0) + 
                     (gunBotAIAdded ? 1 : 0) + (patrolPointsCreated ? 1 : 0);

        total += 4;
        completed += (diamondPrefabCreated ? 1 : 0) + (diamondSpawnerCreated ? 1 : 0) + 
                     (spawnAreasCreated ? 1 : 0) + (diamondsSpawning ? 1 : 0);

        total += 3;
        completed += (gameManagerCreated ? 1 : 0) + (levelConfigAdded ? 1 : 0) + 
                     (totalDiamondsSet ? 1 : 0);

        total += 6;
        completed += (canvasCreated ? 1 : 0) + (gameUICreated ? 1 : 0) + 
                     (healthUICreated ? 1 : 0) + (winPanelCreated ? 1 : 0) + 
                     (gameOverPanelCreated ? 1 : 0) + (pauseMenuCreated ? 1 : 0);

        total += 3;
        completed += (mainMenuInBuild ? 1 : 0) + (level1InBuild ? 1 : 0) + 
                     (inputSystemConfigured ? 1 : 0);

        total += 6;
        completed += (playerCanMove ? 1 : 0) + (gunBotPatrols ? 1 : 0) + 
                     (diamondsCollect ? 1 : 0) + (healthDecreases ? 1 : 0) + 
                     (winConditionWorks ? 1 : 0) + (pauseMenuWorks ? 1 : 0);

        float percentage = (completed / (float)total) * 100f;

        string progressBar = GenerateProgressBar(percentage);

        Debug.Log($@"
╔══════════════════════════════════════════════════════════╗
║              📋 SETUP PROGRESS REPORT 📋                ║
╚══════════════════════════════════════════════════════════╝

{progressBar}

Completed: {completed} / {total} tasks ({percentage:F1}%)

BREAKDOWN:
  Scene Setup:       {GetSectionProgress(level1SceneCreated, roomModelAdded, navMeshBaked, lightingSetup)}
  Player Setup:      {GetSectionProgress(zombieAdded, playerTagSet, playerControllerAdded, playerHealthAdded, cameraSetup)}
  Enemy Setup:       {GetSectionProgress(gunBotAdded, gunBotScaled, gunBotAIAdded, patrolPointsCreated)}
  Diamond System:    {GetSectionProgress(diamondPrefabCreated, diamondSpawnerCreated, spawnAreasCreated, diamondsSpawning)}
  Game Manager:      {GetSectionProgress(gameManagerCreated, levelConfigAdded, totalDiamondsSet)}
  UI Setup:          {GetSectionProgress(canvasCreated, gameUICreated, healthUICreated, winPanelCreated, gameOverPanelCreated, pauseMenuCreated)}
  Build Settings:    {GetSectionProgress(mainMenuInBuild, level1InBuild, inputSystemConfigured)}
  Testing:           {GetSectionProgress(playerCanMove, gunBotPatrols, diamondsCollect, healthDecreases, winConditionWorks, pauseMenuWorks)}

{(percentage >= 100f ? "🎉 CONGRATULATIONS! Setup is 100% complete!" : 
  percentage >= 75f ? "👍 Almost there! Just a few more steps." :
  percentage >= 50f ? "💪 Good progress! Keep going!" :
  percentage >= 25f ? "🚀 You're on your way! Continue setup." :
  "📖 Just getting started! Use Quick Setup Menu.")}

TIP: Use 'MidTerm Game > Quick Setup Menu' for automated setup!
");
    }

    string GetSectionProgress(params bool[] items)
    {
        int completed = 0;
        foreach (bool item in items)
        {
            if (item) completed++;
        }
        
        float percent = (completed / (float)items.Length) * 100f;
        string bar = GenerateProgressBar(percent, 10);
        
        return $"{bar} {completed}/{items.Length}";
    }

    string GenerateProgressBar(float percentage, int length = 30)
    {
        int filled = Mathf.RoundToInt((percentage / 100f) * length);
        string bar = "[";
        
        for (int i = 0; i < length; i++)
        {
            bar += i < filled ? "█" : "░";
        }
        
        bar += $"] {percentage:F1}%";
        return bar;
    }

    [ContextMenu("Reset Checklist")]
    void ResetChecklist()
    {
        level1SceneCreated = false;
        roomModelAdded = false;
        navMeshBaked = false;
        lightingSetup = false;
        zombieAdded = false;
        playerTagSet = false;
        playerControllerAdded = false;
        playerHealthAdded = false;
        cameraSetup = false;
        gunBotAdded = false;
        gunBotScaled = false;
        gunBotAIAdded = false;
        patrolPointsCreated = false;
        diamondPrefabCreated = false;
        diamondSpawnerCreated = false;
        spawnAreasCreated = false;
        diamondsSpawning = false;
        gameManagerCreated = false;
        levelConfigAdded = false;
        totalDiamondsSet = false;
        canvasCreated = false;
        gameUICreated = false;
        healthUICreated = false;
        winPanelCreated = false;
        gameOverPanelCreated = false;
        pauseMenuCreated = false;
        mainMenuInBuild = false;
        level1InBuild = false;
        inputSystemConfigured = false;
        playerCanMove = false;
        gunBotPatrols = false;
        diamondsCollect = false;
        healthDecreases = false;
        winConditionWorks = false;
        pauseMenuWorks = false;
        
        Debug.Log("✅ Checklist reset!");
    }

    [ContextMenu("Mark All Complete")]
    void MarkAllComplete()
    {
        level1SceneCreated = true;
        roomModelAdded = true;
        navMeshBaked = true;
        lightingSetup = true;
        zombieAdded = true;
        playerTagSet = true;
        playerControllerAdded = true;
        playerHealthAdded = true;
        cameraSetup = true;
        gunBotAdded = true;
        gunBotScaled = true;
        gunBotAIAdded = true;
        patrolPointsCreated = true;
        diamondPrefabCreated = true;
        diamondSpawnerCreated = true;
        spawnAreasCreated = true;
        diamondsSpawning = true;
        gameManagerCreated = true;
        levelConfigAdded = true;
        totalDiamondsSet = true;
        canvasCreated = true;
        gameUICreated = true;
        healthUICreated = true;
        winPanelCreated = true;
        gameOverPanelCreated = true;
        pauseMenuCreated = true;
        mainMenuInBuild = true;
        level1InBuild = true;
        inputSystemConfigured = true;
        playerCanMove = true;
        gunBotPatrols = true;
        diamondsCollect = true;
        healthDecreases = true;
        winConditionWorks = true;
        pauseMenuWorks = true;
        
        Debug.Log("✅ All tasks marked complete!");
        ShowProgress();
    }

    [ContextMenu("Open Quick Setup Menu")]
    void OpenQuickSetup()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExecuteMenuItem("MidTerm Game/Quick Setup Menu");
        #endif
    }

    [ContextMenu("Open Validator")]
    void OpenValidator()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExecuteMenuItem("MidTerm Game/Validate Project Setup");
        #endif
    }
}
