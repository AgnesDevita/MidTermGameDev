using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Names - Must Match Build Settings")]
    [SerializeField] string level1SceneName = "Level1";
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string settingsSceneName = "Settings";
    [SerializeField] string creditsSceneName = "Credits";

    public void LoadLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level1SceneName, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
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

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
