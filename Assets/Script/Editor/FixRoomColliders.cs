using UnityEngine;
using UnityEditor;

public class FixRoomColliders : EditorWindow
{
    [MenuItem("MidTerm Game/Fix Room Collision Warnings")]
    static void ShowWindow()
    {
        GetWindow<FixRoomColliders>("Fix Room Colliders");
    }

    void OnGUI()
    {
        GUILayout.Label("🔧 FIX ROOM COLLISION WARNINGS", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Warning: 'Material4 mesh must have at least one non-degenerate triangle'\n\n" +
            "Ini error dari Room model yang punya Mesh Collider dengan mesh lines/points.\n" +
            "Klik tombol di bawah untuk fix!", 
            MessageType.Warning);

        GUILayout.Space(10);

        if (GUILayout.Button("✅ FIX ALL ROOM COLLIDERS", GUILayout.Height(40)))
        {
            FixAllRoomColliders();
        }

        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Tool ini akan:\n" +
            "1. Cari semua GameObject dengan nama 'Material4'\n" +
            "2. Remove MeshCollider yang bermasalah\n" +
            "3. Console jadi bersih!\n\n" +
            "Room collision tetap work karena Room parent sudah ada collider.", 
            MessageType.Info);
    }

    void FixAllRoomColliders()
    {
        int fixedCount = 0;
        
        // Find all GameObjects with "Material4" in name
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Material4"))
            {
                MeshCollider meshCol = obj.GetComponent<MeshCollider>();
                if (meshCol != null)
                {
                    // Check if mesh is problematic
                    Mesh mesh = meshCol.sharedMesh;
                    if (mesh != null && mesh.GetTopology(0) != MeshTopology.Triangles)
                    {
                        Debug.Log($"🔧 Removing problematic MeshCollider from: {obj.name}");
                        DestroyImmediate(meshCol);
                        fixedCount++;
                        
                        EditorUtility.SetDirty(obj);
                    }
                }
            }
        }

        // Also check for any other objects with line/point meshes
        MeshCollider[] allMeshColliders = GameObject.FindObjectsOfType<MeshCollider>();
        foreach (MeshCollider mc in allMeshColliders)
        {
            if (mc.sharedMesh != null)
            {
                try
                {
                    MeshTopology topology = mc.sharedMesh.GetTopology(0);
                    if (topology != MeshTopology.Triangles)
                    {
                        Debug.LogWarning($"⚠️ Found non-triangle mesh collider on: {mc.gameObject.name}");
                        if (EditorUtility.DisplayDialog("Fix Collider?", 
                            $"GameObject '{mc.gameObject.name}' has a mesh collider with {topology} topology.\n\n" +
                            "Remove this MeshCollider?", 
                            "Yes, Remove", "Skip"))
                        {
                            DestroyImmediate(mc);
                            fixedCount++;
                            EditorUtility.SetDirty(mc.gameObject);
                        }
                    }
                }
                catch
                {
                    // Mesh might not have valid topology
                }
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        if (fixedCount > 0)
        {
            EditorUtility.DisplayDialog("Success!", 
                $"✅ Fixed {fixedCount} problematic mesh collider(s)!\n\n" +
                "Warning seharusnya sudah hilang.\n" +
                "Press Play untuk verify.", 
                "OK");
            Debug.Log($"✅ Fixed {fixedCount} room colliders");
        }
        else
        {
            EditorUtility.DisplayDialog("No Issues Found", 
                "Tidak ada problematic mesh colliders ditemukan.\n\n" +
                "Kalau warning masih muncul, coba:\n" +
                "1. Save scene\n" +
                "2. Restart Unity\n" +
                "3. Run fix lagi", 
                "OK");
        }
    }
}
