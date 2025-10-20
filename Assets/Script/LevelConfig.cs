using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    [Header("Level Detection (Auto)")]
    public int levelNumber = 1;
    
    [Header("Diamond Settings")]
    public int diamondCount = 10;
    public float diamondRespawnDelay = 5f;
    
    [Header("GunBot Difficulty Multipliers")]
    public float gunBotSpeedMultiplier = 1f;
    public float gunBotDetectionMultiplier = 1f;
    public float gunBotDamageMultiplier = 1f;
    public float gunBotAttackSpeedMultiplier = 1f;
    
    void Awake()
    {
        AutoDetectLevel();
        ApplyLevelConfiguration();
    }
    
    void AutoDetectLevel()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (sceneName.Contains("Level1"))
        {
            levelNumber = 1;
            diamondCount = 10;
            gunBotSpeedMultiplier = 1f;
            gunBotDetectionMultiplier = 1f;
            gunBotDamageMultiplier = 1f;
            gunBotAttackSpeedMultiplier = 1f;
        }
        else if (sceneName.Contains("Level2"))
        {
            levelNumber = 2;
            diamondCount = 20;
            gunBotSpeedMultiplier = 2f;
            gunBotDetectionMultiplier = 2f;
            gunBotDamageMultiplier = 2f;
            gunBotAttackSpeedMultiplier = 2f;
        }
        else
        {
            levelNumber = 1;
            diamondCount = 10;
            gunBotSpeedMultiplier = 1f;
        }
        
        Debug.Log($"[LevelConfig] Level {levelNumber} detected: {diamondCount} diamonds, GunBot {gunBotSpeedMultiplier}x aggressive");
    }
    
    void ApplyLevelConfiguration()
    {
        DiamondSpawner spawner = FindFirstObjectByType<DiamondSpawner>();
        if (spawner != null)
        {
            spawner.totalDiamonds = diamondCount;
            spawner.respawnDelay = diamondRespawnDelay;
            Debug.Log($"[LevelConfig] DiamondSpawner configured: {diamondCount} diamonds");
        }
        
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.totalDiamonds = diamondCount;
            Debug.Log($"[LevelConfig] GameManager configured: {diamondCount} diamonds target");
        }
        
        GunBotAI[] gunBots = FindObjectsByType<GunBotAI>(FindObjectsSortMode.None);
        foreach (GunBotAI bot in gunBots)
        {
            bot.patrolSpeed *= gunBotSpeedMultiplier;
            bot.chaseSpeed *= gunBotSpeedMultiplier;
            bot.detectionRadius *= gunBotDetectionMultiplier;
            bot.losePlayerRadius *= gunBotDetectionMultiplier;
            bot.attackDamage = Mathf.RoundToInt(bot.attackDamage * gunBotDamageMultiplier);
            bot.attackCooldown /= gunBotAttackSpeedMultiplier;
            
            Debug.Log($"[LevelConfig] GunBot configured: Speed {gunBotSpeedMultiplier}x, Detection {gunBotDetectionMultiplier}x, Damage {gunBotDamageMultiplier}x");
        }
    }
}
