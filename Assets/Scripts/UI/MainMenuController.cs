using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    void Start()
    {
        ShowMainMenu();
    }

    public void StartGame()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        SceneLoader.LoadScene("Level1");
    }

    public void OpenCredits()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        AudioManager.Instance?.PlaySFX(SFXId.UIClick);
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