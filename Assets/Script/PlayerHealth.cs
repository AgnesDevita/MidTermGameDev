using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Maximum health")]
    public int maxHealth = 100;
    
    [Tooltip("Current health")]
    public int currentHealth;
    
    [Tooltip("Invincibility time after taking damage")]
    public float invincibilityDuration = 1f;
    
    [Header("Visual Feedback")]
    [Tooltip("Material to flash when damaged (optional)")]
    public Material damageMaterial;
    
    [Tooltip("Flash duration")]
    public float flashDuration = 0.1f;
    
    [Header("Audio")]
    [Tooltip("Sound when taking damage")]
    public AudioClip damageSound;
    
    [Tooltip("Sound when dying")]
    public AudioClip deathSound;
    
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    private Renderer[] renderers;
    private Material[] originalMaterials;
    private bool isDead = false;
    
    void Start()
    {
        currentHealth = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
        
        if (renderers.Length > 0)
        {
            originalMaterials = new Material[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                originalMaterials[i] = renderers[i].material;
            }
        }
    }
    
    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        Debug.Log($"Player took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            StartCoroutine(FlashEffect());
        }
        
        HealthUI healthUI = FindFirstObjectByType<HealthUI>();
        if (healthUI != null)
        {
            healthUI.UpdateHealthDisplay();
        }
    }
    
    System.Collections.IEnumerator FlashEffect()
    {
        if (renderers.Length == 0 || damageMaterial == null) yield break;
        
        foreach (var renderer in renderers)
        {
            renderer.material = damageMaterial;
        }
        
        yield return new WaitForSeconds(flashDuration);
        
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
    
    void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("💀 Player died!");
        
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
        
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.GameOver();
        }
        
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
    }
    
    public void Heal(int amount)
    {
        if (isDead) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        Debug.Log($"Player healed {amount}! Health: {currentHealth}/{maxHealth}");
        
        HealthUI healthUI = FindFirstObjectByType<HealthUI>();
        if (healthUI != null)
        {
            healthUI.UpdateHealthDisplay();
        }
    }
    
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    
    public bool IsDead()
    {
        return isDead;
    }
    
    public bool IsInvincible()
    {
        return isInvincible;
    }
}
