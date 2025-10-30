using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    private Button button;
    private SceneLoader sceneLoader;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ExitGame);
    }

    void Start()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    public void ExitGame()
    {
        if (sceneLoader != null)
        {
            sceneLoader.QuitGame();
        }
        else
        {
            Debug.Log("Exit Game!");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
