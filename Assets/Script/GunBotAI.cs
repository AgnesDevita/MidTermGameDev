using UnityEngine;
using UnityEngine.AI;

public class GunBotAI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Target player (Zombie). Leave empty to auto-find by Player tag")]
    public Transform player;
    
    [Tooltip("Patrol waypoints. Leave empty to auto-find Points object")]
    public Transform[] patrolPoints;
    
    [Header("Detection Settings")]
    [Tooltip("Distance to detect and start chasing player")]
    public float detectionRadius = 500f;
    
    [Tooltip("Distance where GunBot loses player and returns to patrol")]
    public float losePlayerRadius = 600f;
    
    [Tooltip("Distance to stop and attack player")]
    public float attackRange = 80f;
    
    [Header("Movement Speed")]
    [Tooltip("Walking speed during patrol")]
    public float patrolSpeed = 150f;
    
    [Tooltip("Running speed when chasing player")]
    public float chaseSpeed = 300f;
    
    [Header("Patrol Behavior")]
    [Tooltip("Time to wait at each patrol point")]
    public float waitTimeAtPoint = 2f;
    
    [Tooltip("If true, picks random waypoint. If false, goes in order")]
    public bool randomPatrol = true;
    
    [Header("Combat Settings")]
    [Tooltip("Damage dealt per attack")]
    public int attackDamage = 10;
    
    [Tooltip("Time between attacks")]
    public float attackCooldown = 1.5f;
    
    [Tooltip("Attack sound effect")]
    public AudioClip attackSound;
    
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private float attackTimer = 0f;
    
    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        FixAgentSizeForScale();
        
        agent.speed = patrolSpeed;
        
        if (player == null)
        {
            GameObject zombieObj = GameObject.FindGameObjectWithTag("Player");
            if (zombieObj != null)
            {
                player = zombieObj.transform;
                Debug.Log("GunBot: Found player by tag - " + zombieObj.name);
            }
            else
            {
                Debug.LogWarning("GunBot: No GameObject with 'Player' tag found!");
                
                GameObject zombieByName = GameObject.Find("Zombie");
                if (zombieByName != null)
                {
                    player = zombieByName.transform;
                    Debug.Log("GunBot: Found player by name - Zombie (fallback)");
                }
                else
                {
                    Debug.LogError("GunBot: Cannot find player! Make sure Zombie exists and has 'Player' tag.");
                }
            }
        }
        
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            GameObject pointsParent = GameObject.Find("Points");
            if (pointsParent != null)
            {
                int childCount = pointsParent.transform.childCount;
                patrolPoints = new Transform[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    patrolPoints[i] = pointsParent.transform.GetChild(i);
                }
                Debug.Log($"GunBot: Auto-found {childCount} patrol points");
            }
            else
            {
                Debug.LogError("GunBot: No patrol points found! Create a 'Points' GameObject with waypoint children.");
            }
        }
        
        agent.speed = patrolSpeed;
        
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }
    
    void Update()
    {
        if (player == null) 
        {
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        switch (currentState)
        {
            case State.Patrol:
                HandlePatrolState(distanceToPlayer);
                break;
                
            case State.Chase:
                HandleChaseState(distanceToPlayer);
                break;
                
            case State.Attack:
                HandleAttackState(distanceToPlayer);
                break;
        }
        
        UpdateAnimations();
    }
    
    void HandlePatrolState(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRadius)
        {
            Debug.Log("GunBot: Player detected! Switching to CHASE mode");
            ChangeState(State.Chase);
            return;
        }
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            
            if (waitTimer >= waitTimeAtPoint)
            {
                waitTimer = 0f;
                GoToNextPatrolPoint();
            }
        }
    }
    
    void HandleChaseState(float distanceToPlayer)
    {
        if (distanceToPlayer > losePlayerRadius)
        {
            Debug.Log("GunBot: Lost player. Returning to PATROL mode");
            ChangeState(State.Patrol);
            return;
        }
        
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("GunBot: Player in range! Switching to ATTACK mode");
            ChangeState(State.Attack);
            return;
        }
        
        agent.SetDestination(player.position);
    }
    
    void HandleAttackState(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange + 0.5f)
        {
            Debug.Log("GunBot: Player escaped! Switching to CHASE mode");
            ChangeState(State.Chase);
            return;
        }
        
        agent.SetDestination(transform.position);
        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            AttackPlayer();
            attackTimer = 0f;
        }
    }
    
    void AttackPlayer()
    {
        if (player == null) return;
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"GunBot attacked! Dealt {attackDamage} damage");
            
            if (attackSound != null)
            {
                AudioSource.PlayClipAtPoint(attackSound, transform.position);
            }
        }
    }
    
    void ChangeState(State newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case State.Patrol:
                agent.speed = patrolSpeed;
                agent.angularSpeed = 120;
                GoToNextPatrolPoint();
                break;
                
            case State.Chase:
                agent.speed = chaseSpeed;
                agent.angularSpeed = 200;
                break;
                
            case State.Attack:
                agent.speed = 0f;
                agent.angularSpeed = 360;
                break;
        }
    }
    
    void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        
        if (randomPatrol)
        {
            currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        }
        else
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
        
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        Debug.Log($"GunBot: Moving to patrol point {currentPatrolIndex + 1}");
    }
    
    void UpdateAnimations()
    {
        if (animator == null) return;
        
        float speed = agent.velocity.magnitude;
        
        bool isMoving = speed > 0.1f;
        bool isRunning = speed > patrolSpeed * 0.5f;
        bool isAttacking = currentState == State.Attack;
        
        animator.SetBool("IsWalking", isMoving);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetFloat("Speed", speed);
        animator.SetFloat("VelocityX", agent.velocity.x);
        animator.SetFloat("VelocityZ", agent.velocity.z);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, losePlayerRadius);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.3f);
                }
            }
        }
    }
    
    void FixAgentSizeForScale()
    {
        float scale = transform.localScale.x;
        float targetEffectiveRadius = 7f;
        
        float newRadius = targetEffectiveRadius / scale;
        float newHeight = (targetEffectiveRadius * 2.5f) / scale;
        
        agent.radius = newRadius;
        agent.height = newHeight;
        
        Debug.Log($"GunBot: Auto-fixed size for scale {scale}x\n" +
                  $"NavMesh Radius: {newRadius:F2} → Effective: {newRadius * scale:F1} units\n" +
                  $"NavMesh Height: {newHeight:F2} → Effective: {newHeight * scale:F1} units\n" +
                  $"(Zombie effective radius ≈ 6 units, GunBot now ≈ 7 units)");
        
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null && !capsule.isTrigger)
        {
            capsule.radius = newRadius;
            capsule.height = newHeight;
        }
    }
}
