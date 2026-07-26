using UnityEngine;

public enum GameState { Playing, Paused, GameOver, LevelClear }

public class GameManager : Singleton<GameManager>
{
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
        HUDController.Instance?.UpdateGemCounter(_gemsCollected, GetGemsRequired());
    }

    // Gem
    public void CollectGem()
    {
        if (currentState != GameState.Playing) return;

        _gemsCollected++;
        int required = GetGemsRequired();
        HUDController.Instance?.UpdateGemCounter(_gemsCollected, required);

        if (_gemsCollected >= required)
        {
            AudioManager.Instance?.PlaySFX(SFXId.LevelClear);
            LevelManager.Instance?.OpenPortal();
        }
    }

    int GetGemsRequired()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogWarning("[GameManager] LevelManager tidak ditemukan di scene ini, gemsRequired default ke 5.");
            return 5;
        }
        return LevelManager.Instance.gemsRequired;
    }

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
                StopPatrolAudio();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                UIManager.Instance?.ShowGameOver();
                AudioManager.Instance?.PlaySFX(SFXId.GameOver);
                StopPatrolAudio();
                break;

            case GameState.LevelClear:
                Time.timeScale = 0f;
                UIManager.Instance?.ShowLevelClear();
                StopPatrolAudio();
                break;
        }
    }

    void StopPatrolAudio() => AudioManager.Instance?.UpdatePatrolProximity(false);

    public void GameOver() => SetState(GameState.GameOver);
    public void LevelClear() => SetState(GameState.LevelClear);
}