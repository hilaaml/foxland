using UnityEngine;

public class PauseInputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (GameManager.Instance == null) return;

            if (GameManager.Instance.currentState == GameState.Playing)
            {
                GameManager.Instance.SetState(GameState.Paused);
                UIManager.Instance?.ShowPauseMenu();
            }
            else if (GameManager.Instance.currentState == GameState.Paused)
            {
                GameManager.Instance.SetState(GameState.Playing);
                UIManager.Instance?.ShowHUD();
            }
        }
    }
}