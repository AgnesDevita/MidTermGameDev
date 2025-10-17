using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject pauseRoot;              // drag: PauseMenu_Canvas
    public string mainMenuSceneName = "MainMenu";
    public bool lockCursorInGameplay = true;  // centang kalau mau cursor terkunci saat main

    bool isPaused;

    void Start()
    {
        if (pauseRoot) pauseRoot.SetActive(false);
        SetGameplayCursor(true); // mulai game: cursor sesuai preferensi
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void SetGameplayCursor(bool gameplay)
    {
        if (gameplay && lockCursorInGameplay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseRoot) pauseRoot.SetActive(true);
        SetGameplayCursor(false); // tampilkan cursor
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseRoot) pauseRoot.SetActive(false);
        SetGameplayCursor(true); // kembali ke mode gameplay
    }

    // Hook tombol UI:
    public void OnClick_Resume() => Resume();

    public void OnClick_RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClick_MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OnClick_Quit() => Application.Quit();
}
