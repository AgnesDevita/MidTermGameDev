using UnityEngine;

public class CompilationTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("✅ All MidTerm scripts compiled successfully for Unity 6!");
        Debug.Log($"Unity Version: {Application.unityVersion}");
        
        Destroy(this);
    }
}
