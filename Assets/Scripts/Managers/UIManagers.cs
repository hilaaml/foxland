using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    public void OnSceneReady()
    {
        FindAllCanvas();
        HideAllCanvas();
        // tampilkan HUD hanya di scene gameplay
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "Level1" || scene == "Level2")
            SetCanvas(_canvasHUD, true);
    }

    // Canvas Finder
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
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas c in all)
            if (c.gameObject.CompareTag(tag)) return c;
        return null;
    }

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

    // Show Panels
    public void ShowHUD()
    {
        HideAllCanvas();
        SetCanvas(_canvasHUD, true);
        AudioManager.Instance?.HideMenuOverlay();
    }

    public void ShowPauseMenu()
    {
        SetCanvas(_canvasHUD, true);
        SetCanvas(_canvasPause, true);
        AudioManager.Instance?.ShowMenuOverlay();
    }

    public void ShowLevelClear()
    {
        HideAllCanvas();
        AudioManager.Instance?.ShowMenuOverlay();
        bool isFinalLevel = SceneManager.GetActiveScene().buildIndex == 3;
        SetCanvas(isFinalLevel ? _canvasFinalClear : _canvasLevelClear, true);
    }

    public void ShowGameOver()
    {
        HideAllCanvas();
        AudioManager.Instance?.ShowMenuOverlay();
        SetCanvas(_canvasGameOver, true);
    }

    // HUD Passthrough
    public void UpdateLives(int currentLives) =>
        HUDController.Instance?.UpdateLives(currentLives);
    public void UpdateGemCounter(int col, int req) =>
        HUDController.Instance?.UpdateGemCounter(col, req);
    public void UpdateEnergyBar(float val) =>
        HUDController.Instance?.UpdateEnergyBar(val);
}