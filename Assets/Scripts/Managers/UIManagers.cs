using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    private Canvas _canvasHUD;
    private Canvas _canvasPause;
    private Canvas _canvasLevelClear;
    private Canvas _canvasFinalClear;
    private Canvas _canvasGameOver;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitAfterFrame());
    }

    IEnumerator InitAfterFrame()
    {
        yield return null;
        FindAllCanvas();
        ShowHUD();
    }

    // ── Find Canvas ──────────────────────────────────
    void FindAllCanvas()
    {
        _canvasHUD = FindCanvasByTag("CanvasHUD");
        _canvasPause = FindCanvasByTag("CanvasPause");
        _canvasLevelClear = FindCanvasByTag("CanvasLevelClear");
        _canvasFinalClear = FindCanvasByTag("CanvasFinalClear");
        _canvasGameOver = FindCanvasByTag("CanvasGameOver");
    }

    Canvas FindCanvasByTag(string tag)
    {
        Canvas[] all = FindObjectsByType<Canvas>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (Canvas c in all)
            if (c.gameObject.CompareTag(tag)) return c;

        return null;
    }

    // ── Canvas Control ───────────────────────────────
    void SetCanvas(Canvas canvas, bool active)
    {
        if (canvas != null) canvas.gameObject.SetActive(active);
    }

    void HideAllCanvas()
    {
        SetCanvas(_canvasHUD, false);
        SetCanvas(_canvasPause, false);
        SetCanvas(_canvasLevelClear, false);
        SetCanvas(_canvasFinalClear, false);
        SetCanvas(_canvasGameOver, false);
    }

    public void ShowHUD()
    {
        HideAllCanvas();
        SetCanvas(_canvasHUD, true);
    }

    public void ShowPauseMenu()
    {
        SetCanvas(_canvasHUD, true);
        SetCanvas(_canvasPause, true);
    }

    public void HidePauseMenu()
    {
        SetCanvas(_canvasPause, false);
    }

    public void ShowLevelClear()
    {
        HideAllCanvas();
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex == 3)
            SetCanvas(_canvasFinalClear, true);
        else
            SetCanvas(_canvasLevelClear, true);
    }

    public void ShowGameOver()
    {
        HideAllCanvas();
        SetCanvas(_canvasGameOver, true);
    }

    // ── HUD Updates — delegate ke HUDController ──────
    public void UpdateLives(int currentLives)
    {
        HUDController.Instance?.UpdateLives(currentLives);
    }

    public void UpdateGemCounter(int collected, int required)
    {
        HUDController.Instance?.UpdateGemCounter(collected, required);
    }

    public void UpdateEnergyBar(float normalizedValue)
    {
        HUDController.Instance?.UpdateEnergyBar(normalizedValue);
    }
}