using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Health bar slider")]
    public Slider healthBar;
    
    [Tooltip("Health text (e.g., 80/100)")]
    public TextMeshProUGUI healthText;
    
    [Tooltip("Health bar fill image")]
    public Image healthBarFill;
    
    [Header("Colors")]
    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    
    private PlayerHealth playerHealth;
    
    void Start()
    {
        AutoLinkReferences();
        FindPlayerHealth();
        UpdateHealthDisplay();
    }
    
    void AutoLinkReferences()
    {
        if (healthBar == null || healthText == null || healthBarFill == null)
        {
            Debug.Log("HealthUI: Auto-linking references...");
            
            Transform healthBarObj = transform.Find("HealthBar");
            if (healthBarObj == null)
            {
                healthBarObj = transform.Find("HealthBarBG");
            }
            
            if (healthBarObj != null)
            {
                if (healthBar == null)
                {
                    healthBar = healthBarObj.GetComponent<Slider>();
                    Debug.Log($"HealthUI: Found Slider on {healthBarObj.name}");
                }
                
                Transform fillObj = healthBarObj.Find("Fill");
                if (fillObj == null)
                {
                    fillObj = healthBarObj.Find("HealthBarFill");
                }
                
                if (fillObj != null && healthBarFill == null)
                {
                    healthBarFill = fillObj.GetComponent<Image>();
                    Debug.Log($"HealthUI: Found Fill Image on {fillObj.name}");
                }
                
                Transform textObj = healthBarObj.Find("HealthText");
                if (textObj != null && healthText == null)
                {
                    healthText = textObj.GetComponent<TextMeshProUGUI>();
                    Debug.Log($"HealthUI: Found HealthText on {textObj.name}");
                }
            }
            
            if (healthBar != null && healthText != null && healthBarFill != null)
            {
                Debug.Log("✅ HealthUI: All references auto-linked successfully!");
            }
            else
            {
                Debug.LogWarning($"⚠️ HealthUI: Missing refs - Bar:{healthBar!=null}, Text:{healthText!=null}, Fill:{healthBarFill!=null}");
            }
        }
    }
    
    void FindPlayerHealth()
    {
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
        
        if (playerHealth == null)
        {
            GameObject zombie = GameObject.Find("Zombie");
            if (zombie != null)
            {
                playerHealth = zombie.GetComponent<PlayerHealth>();
            }
        }
        
        if (playerHealth == null)
        {
            Debug.LogWarning("HealthUI: No PlayerHealth found!");
        }
        else
        {
            Debug.Log($"HealthUI: Found PlayerHealth on {playerHealth.gameObject.name}");
        }
    }
    
    void Update()
    {
        if (playerHealth != null)
        {
            UpdateHealthDisplay();
        }
    }
    
    public void UpdateHealthDisplay()
    {
        if (playerHealth == null) return;
        
        float healthPercent = playerHealth.GetHealthPercentage();
        
        if (healthBar != null)
        {
            healthBar.value = healthPercent;
        }
        
        if (healthText != null)
        {
            healthText.text = $"HP: {playerHealth.currentHealth}/{playerHealth.maxHealth}";
        }
        
        if (healthBarFill != null)
        {
            if (healthPercent > 0.6f)
            {
                healthBarFill.color = fullHealthColor;
            }
            else if (healthPercent > 0.3f)
            {
                healthBarFill.color = midHealthColor;
            }
            else
            {
                healthBarFill.color = lowHealthColor;
            }
        }
    }
}
