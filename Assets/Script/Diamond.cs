using UnityEngine;

public class Diamond : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("Auto scale multiplier to make diamond visible")]
    public float autoScaleMultiplier = 3f;
    
    [Tooltip("Rotation animation - DISABLED for static items")]
    public bool enableRotation = false;
    
    [Tooltip("Rotation speed (degrees per second)")]
    public float rotationSpeed = 0f;
    
    [Tooltip("Float animation - DISABLED for static items")]
    public bool enableFloating = false;
    
    [Tooltip("Floating height")]
    public float floatAmplitude = 0f;
    
    [Tooltip("Floating speed")]
    public float floatSpeed = 0f;
    
    [Header("Collection Settings")]
    [Tooltip("Points awarded when collected")]
    public int pointValue = 10;
    
    [Tooltip("Play sound when collected")]
    public AudioClip collectSound;
    
    private Vector3 startPosition;
    private float floatOffset;
    
    void Start()
    {
        if (transform.localScale.magnitude < 5f)
        {
            transform.localScale *= autoScaleMultiplier;
        }
        
        startPosition = transform.position;
        floatOffset = Random.Range(0f, Mathf.PI * 2f);
        
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }
    
    void Update()
    {
        if (enableRotation)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
        
        if (enableFloating)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed + floatOffset) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectDiamond(other.gameObject);
        }
    }
    
    void CollectDiamond(GameObject player)
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.CollectDiamond(pointValue);
        }
        
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        Destroy(gameObject);
    }
}
