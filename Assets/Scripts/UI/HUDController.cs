using UnityEngine;

public class HUDController : MonoBehaviour
{
    void Start()
    {
        PlayLevelBGM();
    }

    void PlayLevelBGM()
    {
        if (AudioManager.Instance == null) return;

        int buildIndex = UnityEngine.SceneManagement
            .SceneManager.GetActiveScene().buildIndex;

        switch (buildIndex)
        {
            case 2: // Level1
                AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmLevel1);
                break;
            case 3: // Level2
                AudioManager.Instance.PlayBGM(AudioManager.Instance.bgmLevel2);
                break;
        }
    }
}