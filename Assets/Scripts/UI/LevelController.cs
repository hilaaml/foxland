using UnityEngine;

public class LevelController : MonoBehaviour
{
    public void NextLevel()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        SceneLoader.LoadNextScene();
    }

    public void PlayAgain()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        SceneLoader.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        SceneLoader.LoadScene("MainMenu");
    }
}