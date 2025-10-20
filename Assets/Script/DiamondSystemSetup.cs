using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiamondSystemSetup : MonoBehaviour
{
    [Header("Auto-Setup Diamond System")]
    [Tooltip("Click in Inspector: Right-click → Setup Diamond System")]
    public bool setupInstructions = true;

    [ContextMenu("1. Add Diamond Script to All Diamonds")]
    public void AddDiamondScriptToAll()
    {
        Diamond[] existingDiamonds = FindObjectsByType<Diamond>(FindObjectsSortMode.None);
        GameObject[] allDiamonds = GameObject.FindGameObjectsWithTag("Untagged");
        
        int added = 0;
        foreach (var obj in allDiamonds)
        {
            if (obj.name.Contains("Diamond") && obj.GetComponent<Diamond>() == null)
            {
                obj.AddComponent<Diamond>();
                
                BoxCollider collider = obj.GetComponent<BoxCollider>();
                if (collider != null)
                {
                    collider.isTrigger = true;
                }
                
                added++;
            }
        }
        
        Debug.Log($"✅ Added Diamond script to {added} diamonds");
    }

    [ContextMenu("2. Create GameManager")]
    public void CreateGameManager()
    {
        GameManager existing = FindFirstObjectByType<GameManager>();
        if (existing != null)
        {
            Debug.LogWarning("GameManager already exists!");
            return;
        }
        
        GameObject gmObj = new GameObject("_GameManager");
        gmObj.AddComponent<GameManager>();
        
        Debug.Log("✅ Created GameManager GameObject");
    }

    [ContextMenu("3. Create Game UI Canvas")]
    public void CreateGameUICanvas()
    {
        Canvas existingCanvas = FindFirstObjectByType<Canvas>();
        GameObject uiCanvas;
        
        if (existingCanvas != null && existingCanvas.name.Contains("Game"))
        {
            uiCanvas = existingCanvas.gameObject;
            Debug.Log("Using existing Canvas");
        }
        else
        {
            uiCanvas = new GameObject("GameUI_Canvas");
            Canvas canvas = uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiCanvas.AddComponent<CanvasScaler>();
            uiCanvas.AddComponent<GraphicRaycaster>();
            Debug.Log("✅ Created new Canvas");
        }
        
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(uiCanvas.transform);
        TextMeshProUGUI scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.pivot = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(20, -20);
        scoreRect.sizeDelta = new Vector2(300, 50);
        
        scoreText.text = "Score: 0";
        scoreText.fontSize = 32;
        scoreText.color = Color.white;
        scoreText.fontStyle = FontStyles.Bold;
        
        GameObject diamondObj = new GameObject("DiamondCountText");
        diamondObj.transform.SetParent(uiCanvas.transform);
        TextMeshProUGUI diamondText = diamondObj.AddComponent<TextMeshProUGUI>();
        
        RectTransform diamondRect = diamondObj.GetComponent<RectTransform>();
        diamondRect.anchorMin = new Vector2(0, 1);
        diamondRect.anchorMax = new Vector2(0, 1);
        diamondRect.pivot = new Vector2(0, 1);
        diamondRect.anchoredPosition = new Vector2(20, -70);
        diamondRect.sizeDelta = new Vector2(300, 50);
        
        diamondText.text = "💎 0/15";
        diamondText.fontSize = 32;
        diamondText.color = Color.cyan;
        diamondText.fontStyle = FontStyles.Bold;
        
        GameUI gameUI = uiCanvas.AddComponent<GameUI>();
        gameUI.scoreText = scoreText;
        gameUI.diamondCountText = diamondText;
        
        Debug.Log("✅ Created Game UI with Score and Diamond Counter");
    }

    [ContextMenu("4. Setup Complete System (All Steps)")]
    public void SetupCompleteSystem()
    {
        Debug.Log("=== Starting Diamond System Setup ===");
        
        AddDiamondScriptToAll();
        CreateGameManager();
        CreateGameUICanvas();
        
        Debug.Log("=== ✅ Diamond System Setup Complete! ===");
        Debug.Log("Next steps:\n" +
                  "1. Check _GameManager in Hierarchy\n" +
                  "2. Check GameUI_Canvas in Hierarchy\n" +
                  "3. Play and collect diamonds!\n" +
                  "4. Make sure Zombie has 'Player' tag");
    }
}
