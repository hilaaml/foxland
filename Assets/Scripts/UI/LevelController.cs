using UnityEngine;

public class LevelController: MonoBehaviour
{
    // dipanggil tombol Next Level di LevelClearPanel
    public void NextLevel()
    {
        AudioManager.Instance?.PlaySFX("UIClick");

        // reset player stats sebelum pindah scene
        FindFirstObjectByType<PlayerHealth>()?.ResetHealth();
        FindFirstObjectByType<PlayerEnergy>()?.ResetEnergy();

        GameManager.Instance?.PrepareNextLevel();
        SceneLoader.LoadNextScene();
    }

    // dipanggil tombol Play Again di FinalClearPanel
    public void PlayAgain()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        GameManager.Instance?.PrepareNextLevel();
        SceneLoader.LoadScene("Level1");
    }

    // dipanggil tombol Main Menu di FinalClearPanel
    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        GameManager.Instance?.PrepareNextLevel();
        SceneLoader.LoadScene("MainMenu");
    }
}