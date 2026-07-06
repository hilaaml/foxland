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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAllCanvas();
        ShowHUD();
    }

    void FindAllCanvas()
    {
        _canvasHUD = FindCanvasByTag("Canvas_HUD");
        _canvasPause = FindCanvasByTag("Canvas_Pause");
        _canvasLevelClear = FindCanvasByTag("Canvas_LevelClear");
        _canvasFinalClear = FindCanvasByTag("Canvas_FinalClear");
        _canvasGameOver = FindCanvasByTag("Canvas_GameOver");
    }

    Canvas FindCanvasByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);
        if (obj == null)
        {
            Debug.LogWarning($"[UIManager] Tag '{tag}' tidak ditemukan.");
            return null;
        }
        return obj.GetComponent<Canvas>();
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
}