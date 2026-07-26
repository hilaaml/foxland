using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (GameManager.Instance.currentState == GameState.Playing)
                Pause();
            else if (GameManager.Instance.currentState == GameState.Paused)
                Resume();
        }
    }

    public void Pause()
    {
        GameManager.Instance?.SetState(GameState.Paused);
        UIManager.Instance?.ShowPauseMenu();
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
    }

    public void Resume()
    {
        GameManager.Instance?.SetState(GameState.Playing);
        UIManager.Instance?.ShowHUD();
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
    }

    public void Restart()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        SceneLoader.ReloadCurrentScene();
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        SceneLoader.LoadScene("MainMenu");
    }
}