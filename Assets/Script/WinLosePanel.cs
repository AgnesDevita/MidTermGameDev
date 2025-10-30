using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLosePanel : MonoBehaviour
{
    [Header("Panel Type")]
    public bool isWinPanel = true;

    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText;
    public Button restartButton;
    public Button mainMenuButton;
    public Button nextLevelButton;

    [Header("Settings")]
    public string winTitle = "VICTORY!";
    public string loseTitle = "GAME OVER";
    public string winMessage = "All diamonds collected!";
    public string loseMessage = "You died!";

    void Start()
    {
        SetupPanel();
    }

    void SetupPanel()
    {
        if (titleText != null)
        {
            titleText.text = isWinPanel ? winTitle : loseTitle;
        }

        if (messageText != null)
        {
            messageText.text = isWinPanel ? winMessage : loseMessage;
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(isWinPanel);
        }

        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText == null) return;

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            int score = gm.GetScore();
            int diamonds = gm.GetDiamondsCollected();
            int total = gm.GetTotalDiamonds();
            scoreText.text = $"Score: {score}\nDiamonds: {diamonds}/{total}";
        }
    }

    public void OnRestart()
    {
        SceneLoader loader = FindFirstObjectByType<SceneLoader>();
        if (loader != null)
        {
            loader.RestartCurrentLevel();
        }
        else
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }

    public void OnMainMenu()
    {
        SceneLoader loader = FindFirstObjectByType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadMainMenu();
        }
        else
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnNextLevel()
    {
        if (!isWinPanel) return;

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.LoadNextLevel();
        }
        else
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
        }
    }
}
