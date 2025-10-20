using UnityEngine;
using UnityEngine.AI;

public class GunBotAI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Target player (Zombie). Kosongkan untuk auto-find")]
    public Transform player;
    
    [Tooltip("Patrol waypoints. Kosongkan untuk auto-find")]
    public Transform[] patrolPoints;
    
    [Header("Detection Settings")]
    [Tooltip("Distance to detect and start chasing player")]
    public float detectionRadius = 250f;
    
    [Tooltip("Jarak 'sentuhan' untuk membunuh player")]
    public float killDistance = 1.5f; 
    
    [Header("Movement Speed")]
    [Tooltip("Walking speed during patrol")]
    public float patrolSpeed = 1250f;
    
    [Tooltip("Running speed when chasing player")]
    public float chaseSpeed = 1250f;
    
    [Header("Patrol Behavior")]
    [Tooltip("Time to wait at each patrol point")]
    public float waitTimeAtPoint = 2f;
    
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    
    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;
    private bool patrolCompleted = false;
    private bool playerIsKilled = false; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agent.stoppingDistance = 0; 
        
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            float modelHeight = capsule.height;
            float modelCenter = capsule.center.y;
            float calculatedOffset = modelHeight + modelCenter;
            agent.baseOffset = calculatedOffset;
        }
        
        agent.speed = patrolSpeed;
        
        // --- INI BAGIAN PENTING UNTUK ERROR ANDA ---
        if (player == null)
        {
            GameObject zombieObj = GameObject.FindGameObjectWithTag("Player");
            if (zombieObj != null)
            {
                player = zombieObj.transform;
                Debug.Log("GunBot: Ditemukan player via Tag 'Player'");
            }
            else
            {
                Debug.LogError("GunBot: TIDAK MENEMUKAN 'Player'. Pastikan Zombie punya Tag 'Player'!");
            }
        }
        // ---------------------------------------------
        
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
            }
        }
        
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        else
        {
            patrolCompleted = true;
        }
    }
    
    void Update()
    {
        if (player == null || playerIsKilled) 
        {
            if (currentState == State.Chase && !patrolCompleted)
            {
                ChangeState(State.Patrol);
            }
            UpdateAnimations(); // Tetap update animasi agar bisa Idle
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
        }
        
        UpdateAnimations();
    }
    
    void HandlePatrolState(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRadius)
        {
            ChangeState(State.Chase);
            return;
        }
        
        if (patrolCompleted)
        {
            if(agent.hasPath) agent.ResetPath();
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
        if (distanceToPlayer <= killDistance)
        {
            KillPlayer();
            return;
        }
        
        agent.SetDestination(player.position);

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
                
                if (!patrolCompleted)
                {
                    agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                }
                break;
                
            case State.Chase:
                agent.speed = chaseSpeed;
                agent.angularSpeed = 200;
                break;
        }
    }
    
    void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        
        if (currentPatrolIndex < patrolPoints.Length - 1)
        {
            currentPatrolIndex++;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        else
        {
            Debug.Log("GunBot: Reached final patrol point. Patrol complete.");
            patrolCompleted = true;
        }
    }

    void KillPlayer()
    {
        if (playerIsKilled) return; 

        playerIsKilled = true;
        agent.isStopped = true; 
        ChangeState(State.Patrol); 

        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (playerScript != null)
        {
            playerScript.Die();
        }
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
        Gizmos.DrawWireSphere(transform.position, killDistance);
        
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform point in patrolPoints)
            {
                if (point != null) Gizmos.DrawSphere(point.position, 0.3f);
            }
        }
    }
}