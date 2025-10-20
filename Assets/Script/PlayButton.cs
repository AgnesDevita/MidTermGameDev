using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string sceneName = "Level1";

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }

   
}
