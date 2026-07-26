using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    public static void LoadScene(int buildIndex)
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(buildIndex);
        else
            SceneManager.LoadScene(buildIndex);
    }

    public static void ReloadCurrentScene()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.ReloadCurrentScene();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadNextScene()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadNextScene();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}