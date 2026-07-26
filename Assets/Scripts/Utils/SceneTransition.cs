using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : Singleton<SceneTransition>
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    public bool IsFading { get; private set; } = false;

    private Image _overlay;

    protected override void Awake()
    {
        base.Awake();
        CreateOverlay();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void CreateOverlay()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        GameObject overlayGO = new GameObject("FadeOverlay");
        overlayGO.transform.SetParent(transform, false);

        _overlay = overlayGO.AddComponent<Image>();
        _overlay.color = Color.black;
        _overlay.raycastTarget = true;

        RectTransform rt = _overlay.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;

        SetAlpha(1f);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SceneEnterSequence(scene.name));
    }

    IEnumerator SceneEnterSequence(string sceneName)
    {
        // 1. Paksa hitam total + freeze di frame pertama
        SetAlpha(1f);
        _overlay.raycastTarget = true;
        Time.timeScale = 0f;

        // 2. Tunggu 2 frame — semua Awake() & Start() selesai
        yield return null;
        yield return null;

        // 3. Init semua manager secara berurutan
        GameManager.Instance?.OnSceneReady();
        UIManager.Instance?.OnSceneReady();
        AudioManager.Instance?.OnSceneReady(sceneName);
        PlayerController.Instance?.GetComponent<PlayerHealth>()?.OnSceneReady();

        // 4. Satu frame lagi setelah manager init
        yield return null;

        // 5. Fade in
        IsFading = true;
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
        SetAlpha(0f);

        // 6. Unfreeze setelah fade selesai
        _overlay.raycastTarget = false;
        IsFading = false;
        Time.timeScale = 1f;
    }

    // Public API
    public void LoadScene(string sceneName)
    {
        if (IsFading) return;
        StartCoroutine(FadeOutThenLoad(
            SceneManager.LoadSceneAsync(sceneName)));
    }

    public void LoadScene(int buildIndex)
    {
        if (IsFading) return;
        StartCoroutine(FadeOutThenLoad(
            SceneManager.LoadSceneAsync(buildIndex)));
    }

    public void ReloadCurrentScene()
    {
        if (IsFading) return;
        StartCoroutine(FadeOutThenLoad(
            SceneManager.LoadSceneAsync(
                SceneManager.GetActiveScene().buildIndex)));
    }

    public void LoadNextScene()
    {
        if (IsFading) return;
        StartCoroutine(FadeOutThenLoad(
            SceneManager.LoadSceneAsync(
                SceneManager.GetActiveScene().buildIndex + 1)));
    }

    // Core Coroutines
    IEnumerator FadeOutThenLoad(AsyncOperation asyncLoad)
    {
        IsFading = true;
        _overlay.raycastTarget = true;
        Time.timeScale = 1f;

        asyncLoad.allowSceneActivation = false;

        // fade OUT — terang ke gelap
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));
        SetAlpha(1f);

        // tunggu scene siap di background
        while (asyncLoad.progress < 0.9f)
            yield return null;

        // aktifkan scene — OnSceneLoaded - SceneEnterSequence handle sisanya
        asyncLoad.allowSceneActivation = true;
        yield return asyncLoad;
        // IsFading direset di SceneEnterSequence
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, t / duration));
            yield return null;
        }
        SetAlpha(to);
    }

    void SetAlpha(float a)
    {
        if (_overlay == null) return;
        Color c = _overlay.color;
        c.a = a;
        _overlay.color = c;
    }
}