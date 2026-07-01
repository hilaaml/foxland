using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Portal")]
    public GameObject portal;              // drag Portal GameObject di Inspector
    public GameObject portalLockedVFX;    // visual saat portal terkunci (opsional)

    [Header("Player Reference")]
    public PlayerHealth playerHealth;
    public PlayerEnergy playerEnergy;

    private bool _portalOpen = false;

    protected override void Awake()
    {
        // LevelManager tidak perlu persist antar scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // TIDAK memanggil base.Awake() agar tidak DontDestroyOnLoad
    }

    void Start()
    {
        // portal tertutup di awal
        if (portal != null)
            portal.SetActive(false);

        if (portalLockedVFX != null)
            portalLockedVFX.SetActive(true);
    }

    public void OpenPortal()
    {
        _portalOpen = true;

        if (portal != null)
            portal.SetActive(true);

        if (portalLockedVFX != null)
            portalLockedVFX.SetActive(false);
    }

    // dipanggil dari Portal.cs saat Finn menyentuh portal
    public void OnPlayerReachPortal()
    {
        if (!_portalOpen) return;

        GameManager.Instance?.LevelClear();
    }

    // dipanggil dari UIManager tombol Next Level
    public void LoadNextLevel()
    {
        // reset player stats untuk level baru
        playerHealth?.ResetHealth();
        playerEnergy?.ResetEnergy();
        GameManager.Instance?.PrepareNextLevel();

        SceneLoader.LoadNextScene();
    }
}