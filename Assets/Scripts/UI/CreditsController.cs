using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public void OnBackToMainMenuClicked()
    {
        AudioManager.Instance?.PlaySFX("UIClick");
        SceneLoader.LoadScene("MainMenu");
    }
}