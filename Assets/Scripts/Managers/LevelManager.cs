using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Portal")]
    public GameObject portal;

    private bool _portalOpen = false;

    protected override void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    void Start()
    {
        if (portal != null)
            portal.SetActive(false);
    }

    public void OpenPortal()
    {
        _portalOpen = true;
        if (portal != null)
            portal.SetActive(true);
    }

    public void OnPlayerReachPortal()
    {
        if (!_portalOpen) return;
        GameManager.Instance?.LevelClear();
    }

    public void LoadNextLevel()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerHealth>()?.ResetHealth();
            player.GetComponent<PlayerEnergy>()?.ResetEnergy();
        }

        SceneLoader.LoadNextScene();
    }
}