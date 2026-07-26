using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Level Config")]
    [Tooltip("Jumlah gem yang harus dikumpulkan untuk membuka portal di level ini.")]
    public int gemsRequired = 5;
    [Tooltip("Jumlah nyawa (heart) Finn di level ini.")]
    public int maxLives = 5;
    [Tooltip("Centang di level terakhir")]
    public bool isFinalLevel = false;

    [Header("Portal")]
    public GameObject portal;

    private bool _portalOpen = false;

    protected override bool Persistent => false;

    void Start()
    {
        if (portal != null)
            portal.SetActive(false);
    }

    public void OpenPortal()
    {
        _portalOpen = true;
        if (portal != null)
            portal.GetComponent<Portal>()?.Activate();
    }

    public void OnPlayerReachPortal()
    {
        if (!_portalOpen) return;
        GameManager.Instance?.LevelClear();
    }
}