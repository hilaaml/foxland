using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SFXEntry
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    public string Name => clip != null ? clip.name : "";
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("SFX")]
    public SFXEntry[] sfxList;

    [Header("BGM Clips")]
    public AudioClip splashBGM;
    public AudioClip mainMenuBGM;
    public AudioClip level1BGM;
    public AudioClip level2BGM;

    [Header("BGM Settings")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    public float fadeDuration = 0.4f;

    [Header("Patrol SFX")]
    public float patrolHearRadius = 8f;

    private AudioSource _sfxSource;
    private AudioSource _mainSource;
    private AudioSource _overlaySource;
    private AudioSource _patrolSource;

    private Dictionary<string, SFXEntry> _sfxDict;

    private Coroutine _mainFadeCo;
    private Coroutine _overlayFadeCo;
    private Coroutine _patrolCo;

    private bool _patrolPlaying;
    private bool _overlayActive;
    private bool _overlayLoaded;

    protected override void Awake()
    {
        base.Awake();

        _sfxSource = CreateSource(false, 1f);
        _mainSource = CreateSource(true, 0f);
        _overlaySource = CreateSource(true, 0f);
        _patrolSource = CreateSource(true, 0f);

        _sfxDict = new Dictionary<string, SFXEntry>();
        foreach (var entry in sfxList)
            if (entry.clip != null && !_sfxDict.ContainsKey(entry.Name))
                _sfxDict[entry.Name] = entry;
    }

    AudioSource CreateSource(bool loop, float startVolume)
    {
        var src = gameObject.AddComponent<AudioSource>();
        src.loop = loop;
        src.volume = startVolume;
        src.playOnAwake = false;
        return src;
    }

    public void OnSceneReady(string sceneName)
    {
        StopPatrol();

        // reset overlay state antar scene
        _overlaySource.Stop();
        _overlaySource.volume = 0f;
        _overlayLoaded = false;
        _overlayActive = false;

        switch (sceneName)
        {
            case "SplashScreen": PlayMainOnce(splashBGM); break;
            case "MainMenu": CrossfadeMain(mainMenuBGM); break;
            case "Level1": CrossfadeMain(level1BGM); break;
            case "Level2": CrossfadeMain(level2BGM); break;
            default: StopMain(); break;
        }
    }

    // BGM Internal
    void PlayMainOnce(AudioClip clip)
    {
        if (_mainFadeCo != null) StopCoroutine(_mainFadeCo);
        _mainSource.loop = false;
        _mainSource.Stop();
        if (clip == null) return;
        _mainSource.clip = clip;
        _mainSource.volume = bgmVolume;
        _mainSource.Play();
    }

    void CrossfadeMain(AudioClip clip)
    {
        _mainSource.loop = true;
        if (_mainFadeCo != null) StopCoroutine(_mainFadeCo);
        _mainFadeCo = StartCoroutine(FadeToClip(_mainSource, clip));
    }

    void StopMain()
    {
        if (_mainFadeCo != null) StopCoroutine(_mainFadeCo);
        _mainSource.Stop();
        _mainSource.volume = 0f;
    }

    IEnumerator FadeToClip(AudioSource source, AudioClip newClip)
    {
        if (source.isPlaying)
            yield return StartCoroutine(FadeVolume(source, 0f));

        source.Stop();
        if (newClip == null) yield break;

        source.clip = newClip;
        source.volume = 0f;
        source.Play();
        yield return StartCoroutine(FadeVolume(source, bgmVolume));
    }

    // Panel Overlay BGM
    public void ShowMenuOverlay()
    {
        if (_overlayActive) return;
        _overlayActive = true;

        if (_mainFadeCo != null) StopCoroutine(_mainFadeCo);
        _mainFadeCo = StartCoroutine(FadeVolume(_mainSource, 0f));

        if (_overlayFadeCo != null) StopCoroutine(_overlayFadeCo);

        if (!_overlayLoaded)
        {
            _overlaySource.clip = mainMenuBGM;
            _overlaySource.volume = 0f;
            _overlaySource.Play();
            _overlayLoaded = true;
        }
        else
        {
            _overlaySource.UnPause();
        }

        _overlayFadeCo = StartCoroutine(FadeVolume(_overlaySource, bgmVolume));
    }

    public void HideMenuOverlay()
    {
        if (!_overlayActive) return;
        _overlayActive = false;

        if (_overlayFadeCo != null) StopCoroutine(_overlayFadeCo);
        _overlayFadeCo = StartCoroutine(FadeOutThenPause(_overlaySource));

        if (_mainFadeCo != null) StopCoroutine(_mainFadeCo);
        _mainFadeCo = StartCoroutine(FadeVolume(_mainSource, bgmVolume));
    }

    IEnumerator FadeOutThenPause(AudioSource source)
    {
        yield return StartCoroutine(FadeVolume(source, 0f));
        source.Pause();
    }

    // SFX
    public void PlaySFX(string soundName)
    {
        if (_sfxDict.TryGetValue(soundName, out SFXEntry entry))
            _sfxSource.PlayOneShot(entry.clip, entry.volume);
        else
            Debug.LogWarning($"[AudioManager] SFX '{soundName}' tidak ditemukan!");
    }

    // Patrol SFX Proximity
    public void UpdatePatrolProximity(bool anyNearby)
    {
        if (!_sfxDict.TryGetValue("PatrolSFX", out SFXEntry entry)) return;

        if (anyNearby && !_patrolPlaying)
        {
            _patrolSource.clip = entry.clip;
            _patrolSource.volume = 0f;
            _patrolSource.Play();
            _patrolPlaying = true;

            if (_patrolCo != null) StopCoroutine(_patrolCo);
            _patrolCo = StartCoroutine(FadeVolume(_patrolSource, entry.volume));
        }
        else if (!anyNearby && _patrolPlaying)
        {
            if (_patrolCo != null) StopCoroutine(_patrolCo);
            _patrolCo = StartCoroutine(FadeOutPatrol());
        }
    }

    public void StopPatrol()
    {
        if (_patrolCo != null) StopCoroutine(_patrolCo);
        _patrolPlaying = false;
        _patrolSource.Stop();
        _patrolSource.volume = 0f;
    }

    IEnumerator FadeOutPatrol()
    {
        _patrolPlaying = false;
        yield return StartCoroutine(FadeVolume(_patrolSource, 0f));
        _patrolSource.Stop();
    }

    // Utilities
    IEnumerator FadeVolume(AudioSource source, float target)
    {
        float start = source.volume;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        source.volume = target;
    }

    public void SetBGMVolume(float vol)
    {
        bgmVolume = vol;
        if (_mainSource.isPlaying && !_overlayActive)
            _mainSource.volume = vol;
    }

    public float GetPatrolHearRadius() => patrolHearRadius;
}