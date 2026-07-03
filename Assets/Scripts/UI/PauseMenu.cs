using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Update()
    {
        // toggle pause dengan Escape atau P
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
        AudioManager.Instance?.PlaySFX("UIClick");
    }

    public void Resume()
    {
        GameManager.Instance?.SetState(GameState.Playing);
        UIManager.Instance?.ShowHUD();
        AudioManager.Instance?.PlaySFX("UIClick");
    }

    public void Restart()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.ReloadCurrentScene();
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("MainMenu");
    }
}