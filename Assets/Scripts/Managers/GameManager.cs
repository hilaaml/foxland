using UnityEngine;

public enum GameState { Playing, Paused, GameOver, LevelClear }

public class GameManager : Singleton<GameManager>
{
    [Header("Gem Settings")]
    public int gemsRequired = 5;
    private int _gemsCollected = 0;

    public GameState currentState { get; private set; } = GameState.Playing;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnSceneReady()
    {
        _gemsCollected = 0;
        currentState = GameState.Playing;
        HUDController.Instance?.UpdateGemCounter(_gemsCollected, gemsRequired);
    }

    // Gem
    public void CollectGem()
    {
        if (currentState != GameState.Playing) return;

        _gemsCollected++;
        HUDController.Instance?.UpdateGemCounter(_gemsCollected, gemsRequired);

        if (_gemsCollected >= gemsRequired)
        {
            AudioManager.Instance?.PlaySFX("LevelClear");
            LevelManager.Instance?.OpenPortal();
        }
    }

    public int GetGemsCollected() => _gemsCollected;

    // State Machine
    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Playing:
                if (SceneTransition.Instance == null || !SceneTransition.Instance.IsFading)
                    Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                UIManager.Instance?.ShowGameOver();
                AudioManager.Instance?.PlaySFX("GameOver");
                break;

            case GameState.LevelClear:
                Time.timeScale = 0f;
                UIManager.Instance?.ShowLevelClear();
                break;
        }
    }

    public void GameOver() => SetState(GameState.GameOver);
    public void LevelClear() => SetState(GameState.LevelClear);
}