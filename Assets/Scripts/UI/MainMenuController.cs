using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    void Start()
    {
        ShowMainMenu();
        AudioManager.Instance?.PlayBGM(
            AudioManager.Instance.bgmMainMenu);
    }

    public void StartGame()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("Level1");
    }

    public void OpenCredits()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
}