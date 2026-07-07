using UnityEngine;

public class DontDestroyEventSystem : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}