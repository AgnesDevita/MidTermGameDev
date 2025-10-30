using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour
{
    public string sceneName = "Level1";
    private Button button;
    private SceneLoader sceneLoader;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayGame);
    }

    void Start()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    public void PlayGame()
    {
        if (sceneLoader != null)
        {
            sceneLoader.LoadLevel1();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
