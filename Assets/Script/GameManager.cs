using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    [Tooltip("Total diamonds needed to win")]
    public int totalDiamonds = 15;
    
    [Tooltip("Show win screen when all diamonds collected")]
    public bool autoWinOnComplete = true;
    
    [Header("UI References")]
    [Tooltip("Text showing current score (e.g., Score: 150)")]
    public TextMeshProUGUI scoreText;
    
    [Tooltip("Text showing diamonds collected (e.g., Diamonds: 5/15)")]
    public TextMeshProUGUI diamondCountText;
    
    [Tooltip("Win screen panel")]
    public GameObject winPanel;
    
    [Tooltip("Game Over panel (optional)")]
    public GameObject gameOverPanel;
    
    [Header("Audio")]
    public AudioClip winSound;
    public AudioClip gameOverSound;
    
    private int currentScore = 0;
    private int diamondsCollected = 0;
    private bool gameEnded = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        if (winPanel) winPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        
        LevelConfig levelConfig = FindFirstObjectByType<LevelConfig>();
        if (levelConfig != null)
        {
            totalDiamonds = levelConfig.diamondCount;
            
            int savedScore, savedDiamonds;
            GameProgress.LoadProgress(out savedScore, out savedDiamonds);
            
            currentScore = savedScore;
            diamondsCollected = savedDiamonds;
            
            Debug.Log($"GameManager: Level {levelConfig.levelNumber}, Target: {totalDiamonds}, Current: {diamondsCollected}/{totalDiamonds}");
        }
        else
        {
            int diamondsInScene = FindObjectsByType<Diamond>(FindObjectsSortMode.None).Length;
            if (diamondsInScene > 0)
            {
                totalDiamonds = diamondsInScene;
                Debug.Log($"GameManager: Found {totalDiamonds} diamonds in scene (no LevelConfig)");
            }
        }
        
        UpdateUI();
    }
    
    public void CollectDiamond(int points)
    {
        if (gameEnded) return;
        
        currentScore += points;
        diamondsCollected++;
        
        GameProgress.SaveProgress(currentScore, diamondsCollected);
        
        Debug.Log($"Diamond collected! Score: {currentScore}, Diamonds: {diamondsCollected}/{totalDiamonds}");
        
        UpdateUI();
        
        if (diamondsCollected >= totalDiamonds && autoWinOnComplete)
        {
            LevelConfig levelConfig = FindFirstObjectByType<LevelConfig>();
            if (levelConfig != null && levelConfig.levelNumber == 1)
            {
                LoadNextLevel();
            }
            else
            {
                WinGame();
            }
        }
    }
    
    void UpdateUI()
    {
        if (scoreText)
        {
            scoreText.text = $"Score: {currentScore}";
        }
        
        if (diamondCountText)
        {
            diamondCountText.text = $"Diamonds: {diamondsCollected}/{totalDiamonds}";
        }
    }
    
    public void WinGame()
    {
        if (gameEnded) return;
        
        gameEnded = true;
        Debug.Log("🎉 YOU WIN! All diamonds collected!");
        
        GameProgress.ResetProgress();
        
        if (winPanel)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        
        if (winSound)
        {
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
        }
    }
    
    public void LoadNextLevel()
    {
        Debug.Log("🎮 Level Complete! Loading next level...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }
    
    public void GameOver()
    {
        if (gameEnded) return;
        
        gameEnded = true;
        Debug.Log("💀 GAME OVER!");
        
        GameProgress.ResetProgress();
        
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        
        if (gameOverSound)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position);
        }
    }
    
    public void RestartGame()
    {
        GameProgress.ResetProgress();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public int GetScore() => currentScore;
    public int GetDiamondsCollected() => diamondsCollected;
    public int GetTotalDiamonds() => totalDiamonds;
    public bool IsGameEnded() => gameEnded;
}
