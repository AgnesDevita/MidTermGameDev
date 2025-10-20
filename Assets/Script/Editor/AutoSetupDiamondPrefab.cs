using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class AutoSetupDiamondPrefab
{
    private const string PREFAB_SETUP_KEY = "DiamondPrefab_AutoSetup_Done_v1";
    
    static AutoSetupDiamondPrefab()
    {
        EditorApplication.delayCall += CheckAndSetupPrefab;
    }
    
    static void CheckAndSetupPrefab()
    {
        bool alreadySetup = EditorPrefs.GetBool(PREFAB_SETUP_KEY, false);
        
        if (alreadySetup)
        {
            return;
        }
        
        SetupDiamondPrefab();
        
        EditorPrefs.SetBool(PREFAB_SETUP_KEY, true);
    }
    
    [MenuItem("Tools/Diamond System/Setup Diamond Prefab")]
    static void SetupDiamondPrefab()
    {
        string prefabPath = "Assets/UTS/MidTermGameDev/Assets/Asset/Diamond.prefab";
        
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            Debug.LogWarning($"Diamond prefab not found at {prefabPath}");
            return;
        }
        
        GameObject prefabInstance = PrefabUtility.LoadPrefabContents(prefabPath);
        
        bool modified = false;
        
        if (prefabInstance.GetComponent<Diamond>() == null)
        {
            prefabInstance.AddComponent<Diamond>();
            modified = true;
            Debug.Log("✅ Added Diamond script to prefab");
        }
        
        BoxCollider collider = prefabInstance.GetComponent<BoxCollider>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
            modified = true;
            Debug.Log("✅ Set BoxCollider as trigger");
        }
        
        if (modified)
        {
            PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabPath);
            Debug.Log("💾 Diamond prefab saved!");
        }
        else
        {
            Debug.Log("⚠️ Diamond prefab already setup");
        }
        
        PrefabUtility.UnloadPrefabContents(prefabInstance);
    }
}
