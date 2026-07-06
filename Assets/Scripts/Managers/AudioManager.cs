using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundEntry
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("SFX")]
    public SoundEntry[] sfxList;

    [Header("BGM")]
    public AudioClip bgmMainMenu;
    public AudioClip bgmLevel1;
    public AudioClip bgmLevel2;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;

    private AudioSource _sfxSource;
    private AudioSource _bgmSource;
    private Dictionary<string, SoundEntry> _sfxDict;

    protected override void Awake()
    {
        base.Awake();

        // buat 2 AudioSource: satu untuk SFX, satu untuk BGM
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.volume = bgmVolume;

        // build dictionary untuk lookup O(1)
        _sfxDict = new Dictionary<string, SoundEntry>();
        foreach (var entry in sfxList)
        {
            if (!_sfxDict.ContainsKey(entry.name))
                _sfxDict[entry.name] = entry;
        }
    }

    // SFX
    public void PlaySFX(string soundName)
    {
        if (_sfxDict.TryGetValue(soundName, out SoundEntry entry))
        {
            _sfxSource.PlayOneShot(entry.clip, entry.volume);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] SFX '{soundName}' tidak ditemukan!");
        }
    }

    // BGM
    public void PlayBGM(AudioClip clip)
    {
        if (_bgmSource.clip == clip) return;
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void StopBGM() => _bgmSource.Stop();

    public void SetBGMVolume(float vol) => _bgmSource.volume = vol;
}