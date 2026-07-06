using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        GameManager.Instance?.PrepareNextLevel();
        SceneLoader.LoadScene("MainMenu");
    }

    public void Retry()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        GameManager.Instance?.PrepareNextLevel();
        SceneLoader.LoadScene("Level1"); // selalu dari Level1 sesuai GDD
    }
}