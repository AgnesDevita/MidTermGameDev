using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DiamondSpawner : MonoBehaviour
{
    [Header("What to spawn")]
    public GameObject diamondPrefab;
    public int totalDiamonds = 20;
    
    [Header("Auto Spawn Settings")]
    public bool autoSpawnOnStart = true;
    public bool enableRespawn = true;
    public float respawnDelay = 5f;

    [Header("Where to spawn (add BoxCollider per ruangan)")]
    public List<BoxCollider> spawnAreas = new List<BoxCollider>();

    [Header("Placement rules")]
    public float minSpacing = 2.0f;
    public LayerMask groundMask;
    public LayerMask obstacleMask;
    public float raycastHeight = 10f;
    public float heightOffset = 0.5f;
    public int maxAttemptsPerDiamond = 100;

    [Header("Optional")]
    public bool alignToGroundNormal = false;
    public bool useNavMeshCheck = true;
    public float navMeshMaxDistance = 2f;

    private readonly List<Vector3> placedPositions = new List<Vector3>();
    private int currentDiamondCount = 0;
    private float respawnTimer = 0f;

    void Start()
    {
        if (autoSpawnOnStart)
        {
            SpawnNow();
        }
    }
    
    void Update()
    {
        if (!enableRespawn) return;
        
        currentDiamondCount = transform.childCount;
        
        if (currentDiamondCount < totalDiamonds)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= respawnDelay)
            {
                int needed = totalDiamonds - currentDiamondCount;
                Debug.Log($"Respawning {needed} diamonds...");
                SpawnDiamonds(needed);
                respawnTimer = 0f;
            }
        }
        else
        {
            respawnTimer = 0f;
        }
    }

    [ContextMenu("Spawn Diamonds Now")]
    public void SpawnNow()
    {
        ClearAllDiamonds();
        placedPositions.Clear();
        SpawnDiamonds(totalDiamonds);
    }
    
    void ClearAllDiamonds()
    {
        var toDelete = new List<Transform>();
        foreach (Transform child in transform) toDelete.Add(child);
        foreach (var t in toDelete)
        {
            if (Application.isPlaying)
                Destroy(t.gameObject);
            else
                DestroyImmediate(t.gameObject);
        }
    }

    void SpawnDiamonds(int count)
    {
        if (diamondPrefab == null)
        {
            Debug.LogError("diamondPrefab belum di-assign.");
            return;
        }
        if (spawnAreas == null || spawnAreas.Count == 0)
        {
            Debug.LogError("Tambahkan minimal satu BoxCollider ke 'spawnAreas'.");
            return;
        }

        int spawned = 0;
        int safety = 0;

        while (spawned < count && safety < count * maxAttemptsPerDiamond)
        {
            safety++;

            var area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            if (area == null) continue;

            Vector3 randomWorld = GetRandomPointInsideBox(area);

            Vector3 start = randomWorld + Vector3.up * raycastHeight;
            if (!Physics.Raycast(start, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundMask))
                continue;

            Vector3 candidate = hit.point + Vector3.up * heightOffset;

            if (Physics.CheckSphere(candidate, 0.3f, obstacleMask))
                continue;

            bool tooClose = false;
            foreach (var p in placedPositions)
            {
                if (Vector3.SqrMagnitude(p - candidate) < minSpacing * minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose) continue;

            if (useNavMeshCheck)
            {
                if (!NavMesh.SamplePosition(candidate, out NavMeshHit nHit, navMeshMaxDistance, NavMesh.AllAreas))
                    continue;
                candidate = nHit.position + Vector3.up * heightOffset;
            }

            var rot = alignToGroundNormal ? Quaternion.FromToRotation(Vector3.up, hit.normal) : Quaternion.identity;
            var go = Instantiate(diamondPrefab, candidate, rot, this.transform);
            
            if (go.GetComponent<Diamond>() == null)
            {
                go.AddComponent<Diamond>();
            }
            
            placedPositions.Add(candidate);
            spawned++;
        }

        if (spawned < count)
        {
            Debug.LogWarning($"Hanya berhasil spawn {spawned}/{count}. Tambah area atau kurangi minSpacing.");
        }
        else
        {
            Debug.Log($"✅ Sukses spawn {spawned} diamond di posisi NavMesh walkable dengan height offset {heightOffset}");
        }
    }

    private Vector3 GetRandomPointInsideBox(BoxCollider box)
    {
        Vector3 extents = box.size * 0.5f;

        Vector3 local = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        local += box.center;
        return box.transform.TransformPoint(local);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.15f);
        foreach (var area in spawnAreas)
        {
            if (area == null) continue;
            Matrix4x4 m = Matrix4x4.TRS(area.transform.TransformPoint(area.center), area.transform.rotation, area.transform.lossyScale);
            Gizmos.matrix = m;
            Gizmos.DrawCube(Vector3.zero, area.size);
            Gizmos.matrix = Matrix4x4.identity;
        }

        Gizmos.color = Color.yellow;
        foreach (var p in placedPositions)
            Gizmos.DrawWireSphere(p, 0.15f);
    }
}
