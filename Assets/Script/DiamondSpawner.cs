// DiamondSpawner.cs
using System.Collections.Generic;
using UnityEngine;
#if UNITY_AI_NAVIGATION
using UnityEngine.AI; // optional kalau mau cek NavMesh
#endif

public class DiamondSpawner : MonoBehaviour
{
    [Header("What to spawn")]
    public GameObject diamondPrefab;
    public int totalDiamonds = 20;

    [Header("Where to spawn (add BoxCollider per ruangan)")]
    public List<BoxCollider> spawnAreas = new List<BoxCollider>();

    [Header("Placement rules")]
    public float minSpacing = 1.0f;                 // Jarak antar diamond
    public LayerMask groundMask;                    // Layer lantai
    public LayerMask obstacleMask;                  // Layer dinding/halangan
    public float raycastHeight = 5f;                // Ketinggian raycast dari atas
    public int maxAttemptsPerDiamond = 100;         // Biar gak infinite loop

    [Header("Optional")]
    public bool alignToGroundNormal = false;        // Putar mengikuti normal lantai
    public bool useNavMeshCheck = false;            // True jika pakai NavMesh.SamplePosition
    public float navMeshMaxDistance = 0.5f;         // Batas toleransi ke NavMesh

    private readonly List<Vector3> placedPositions = new List<Vector3>();

    [ContextMenu("Spawn Diamonds Now")]
    public void SpawnNow()
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

        // Hapus diamond lama (anak dari spawner)
        var toDelete = new List<Transform>();
        foreach (Transform child in transform) toDelete.Add(child);
        foreach (var t in toDelete) DestroyImmediate(t.gameObject);

        placedPositions.Clear();

        int spawned = 0;
        int safety = 0;

        while (spawned < totalDiamonds && safety < totalDiamonds * maxAttemptsPerDiamond)
        {
            safety++;

            // Pilih area ruangan secara acak
            var area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            if (area == null) continue;

            // Titik acak dalam BoxCollider (local -> world)
            Vector3 randomWorld = GetRandomPointInsideBox(area);

            // Raycast dari atas ke bawah untuk cari lantai
            Vector3 start = randomWorld + Vector3.up * raycastHeight;
            if (!Physics.Raycast(start, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundMask))
                continue; // tidak mengenai lantai

            Vector3 candidate = hit.point;

            // Cek menabrak obstacle? pakai OverlapSphere sangat tipis
            if (Physics.CheckSphere(candidate + Vector3.up * 0.1f, 0.2f, obstacleMask))
                continue;

            // Cek jarak minimal antar diamond
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

            // Optional: cek NavMesh
            if (useNavMeshCheck)
            {
                #if UNITY_AI_NAVIGATION
                if (!NavMesh.SamplePosition(candidate, out NavMeshHit nHit, navMeshMaxDistance, NavMesh.AllAreas))
                    continue;
                candidate = nHit.position;
                #else
                // Jika Unity AI package belum aktif, lewati check
                #endif
            }

            // Spawn
            var rot = alignToGroundNormal ? Quaternion.FromToRotation(Vector3.up, hit.normal) : Quaternion.identity;
            var go = Instantiate(diamondPrefab, candidate, rot, this.transform);

            placedPositions.Add(candidate);
            spawned++;
        }

        if (spawned < totalDiamonds)
        {
            Debug.LogWarning($"Hanya berhasil spawn {spawned}/{totalDiamonds}. " +
                             $"Tambah area, kurangi minSpacing, atau perbesar maxAttemptsPerDiamond.");
        }
        else
        {
            Debug.Log($"Sukses spawn {spawned} diamond.");
        }
    }

    private Vector3 GetRandomPointInsideBox(BoxCollider box)
    {
        // Dapatkan ukuran half-extent pada local space
        Vector3 extents = box.size * 0.5f;

        // Titik acak pada local space, ter-center di box.center
        Vector3 local = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        // Geser ke center collider
        local += box.center;

        // Konversi ke world space
        return box.transform.TransformPoint(local);
    }

    // Gambarkan gizmo area
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
