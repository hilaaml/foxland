using UnityEngine;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup splashPanel;

    [Header("Timing")]
    public float fadeInDuration = 1f;
    public float holdDuration = 2f;
    public float fadeOutDuration = 1f;

    [Header("Next Scene")]
    public string nextSceneName = "MainMenu";

    void Start()
    {
        splashPanel.alpha = 0f;
        StartCoroutine(SplashSequence());
    }

    IEnumerator SplashSequence()
    {
        yield return StartCoroutine(Fade(splashPanel, 0f, 1f, fadeInDuration));

        yield return new WaitForSeconds(holdDuration);

        yield return StartCoroutine(Fade(splashPanel, 1f, 0f, fadeOutDuration));

        SceneLoader.LoadScene(nextSceneName);
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }
}