using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    LevelClear
}

public class GameManager : Singleton<GameManager>
{
    [Header("Gem Settings")]
    public int gemsRequired = 5;
    private int _gemsCollected = 0;

    [Header("State")]
    public GameState currentState = GameState.Playing;

    // referensi portal — di-assign dari LevelManager
    private LevelManager _levelManager;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        ResetGems();
        SetState(GameState.Playing);
    }

    // Gem
    public void CollectGem()
    {
        if (currentState != GameState.Playing) return;

        _gemsCollected++;
        HUDController.Instance?.UpdateGemCounter(_gemsCollected, gemsRequired);

        if (_gemsCollected >= gemsRequired)
        {
            OnAllGemsCollected();
        }
    }

    void OnAllGemsCollected()
    {
        AudioManager.Instance?.PlaySFX("LevelClear");
        LevelManager.Instance?.OpenPortal();
    }

    public int GetGemsCollected() => _gemsCollected;

    public void ResetGems()
    {
        _gemsCollected = 0;
        HUDController.Instance?.UpdateGemCounter(_gemsCollected, gemsRequired);
    }

    // State Machine
    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Playing:
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

    public void GameOver()
    {
        SetState(GameState.GameOver);
    }

    public void LevelClear()
    {
        SetState(GameState.LevelClear);
    }

    // Reset untuk level baru
    public void PrepareNextLevel()
    {
        ResetGems();
        SetState(GameState.Playing);
    }
}