using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI diamondCountText;
    public TextMeshProUGUI timerText;
    
    [Header("Settings")]
    public bool showTimer = false;
    
    private GameManager gameManager;
    private LevelConfig levelConfig;
    private float elapsedTime = 0f;
    
    void Start()
    {
        AutoLinkReferences();
        
        gameManager = FindFirstObjectByType<GameManager>();
        levelConfig = FindFirstObjectByType<LevelConfig>();
        
        if (gameManager == null)
        {
            Debug.LogWarning("GameUI: No GameManager found in scene!");
        }
        
        if (levelConfig != null && levelText)
        {
            levelText.text = $"Level {levelConfig.levelNumber}";
        }
    }
    
    void AutoLinkReferences()
    {
        if (levelText == null)
        {
            Transform obj = transform.Find("LevelText");
            if (obj != null)
            {
                levelText = obj.GetComponent<TextMeshProUGUI>();
                Debug.Log("GameUI: Auto-linked LevelText");
            }
        }
        
        if (scoreText == null)
        {
            Transform obj = transform.Find("ScoreText");
            if (obj != null)
            {
                scoreText = obj.GetComponent<TextMeshProUGUI>();
                Debug.Log("GameUI: Auto-linked ScoreText");
            }
        }
        
        if (diamondCountText == null)
        {
            Transform obj = transform.Find("DiamondCountText");
            if (obj != null)
            {
                diamondCountText = obj.GetComponent<TextMeshProUGUI>();
                Debug.Log("GameUI: Auto-linked DiamondCountText");
            }
        }
    }
    
    void Update()
    {
        if (gameManager == null) return;
        
        if (levelConfig != null && levelText)
        {
            levelText.text = $"Level {levelConfig.levelNumber}";
        }
        
        if (scoreText)
        {
            scoreText.text = $"Score: {gameManager.GetScore()}";
        }
        
        if (diamondCountText)
        {
            diamondCountText.text = $"Diamonds: {gameManager.GetDiamondsCollected()}/{gameManager.GetTotalDiamonds()}";
        }
        
        if (showTimer && timerText && !gameManager.IsGameEnded())
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}
