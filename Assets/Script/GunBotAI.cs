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
    public float detectionRadius = 250f;
    
    [Tooltip("Distance where GunBot loses player and returns to patrol")]
    public float losePlayerRadius = 350f;
    
    [Tooltip("Distance to stop and attack player")]
    public float attackRange = 50f;
    
    [Header("Movement Speed")]
    [Tooltip("Walking speed during patrol")]
    public float patrolSpeed = 1250f;
    
    [Tooltip("Running speed when chasing player")]
    public float chaseSpeed = 1250f;
    
    [Header("Patrol Behavior")]
    [Tooltip("Time to wait at each patrol point")]
    public float waitTimeAtPoint = 2f;
    
    [Tooltip("If true, picks random waypoint. If false, goes in order")]
    public bool randomPatrol = true;
    
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    
    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            float modelHeight = capsule.height;
            float modelCenter = capsule.center.y;
            float calculatedOffset = modelHeight + modelCenter;
            
            agent.baseOffset = calculatedOffset;
            Debug.Log($"GunBot: Auto-set Base Offset to {calculatedOffset} (height={modelHeight}, center={modelCenter})");
        }
        
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
            Debug.LogWarning("GunBot: Player is NULL! Cannot detect player.");
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        Debug.Log($"GunBot: Distance to player = {distanceToPlayer:F1}, Detection radius = {detectionRadius}, Current state = {currentState}");
        
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
        animator.SetFloat("Speed", speed);
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
}
