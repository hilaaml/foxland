using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<T>();
            return _instance;
        }
    }

    // Override jadi false untuk singleton yang scene-scoped
    // (mis. HUDController, LevelManager — tidak boleh DontDestroyOnLoad).
    protected virtual bool Persistent => true;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
        if (Persistent) DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
}