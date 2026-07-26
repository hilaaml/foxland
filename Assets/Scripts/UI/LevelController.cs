using UnityEngine;

public class LevelController: MonoBehaviour
{
    public void NextLevel()
    {
        AudioManager.Instance?.PlaySFX("UIClick");

        FindFirstObjectByType<PlayerHealth>()?.ResetHealth();
        FindFirstObjectByType<PlayerEnergy>()?.ResetEnergy();

        SceneLoader.LoadNextScene();
    }

    public void PlayAgain()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("MainMenu");
    }
}