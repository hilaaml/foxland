using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("MainMenu");
    }

    public void Retry()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("Level1");
    }
}