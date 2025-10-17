using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Nama scene harus sama persis dg di Project")]
    [SerializeField] string gameSceneName = "Game";
    [SerializeField] string settingsSceneName = "Settings";   // kalau Settings pakai scene terpisah
    [SerializeField] string creditsSceneName = "Credits";     // opsional

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Single);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(creditsSceneName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
