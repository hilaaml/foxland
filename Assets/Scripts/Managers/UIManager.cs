using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("HUD")]
    public Image heartBarImage;
    public Sprite[] heartSprites;
    public TextMeshProUGUI gemCounterText;
    public Slider energyBar;

    [Header("Panels")]
    public GameObject hudPanel;
    public GameObject pauseMenuPanel;
    public GameObject levelClearPanel;   // Level 1
    public GameObject finalClearPanel;   // Level 2
    public GameObject gameOverPanel;

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
        ShowHUD();
    }

    // HUD Updates
    public void UpdateLives(int currentLives)
    {
        if (heartBarImage != null && heartSprites != null && currentLives >= 0 && currentLives < heartSprites.Length)
        {
            heartBarImage.sprite = heartSprites[currentLives];
        }
    }

    public void UpdateGemCounter(int collected, int required)
    {
        if (gemCounterText != null)
            gemCounterText.text = $"{collected}/{required}";
    }

    public void UpdateEnergyBar(float normalizedValue)
    {
        if (energyBar != null)
            energyBar.value = normalizedValue;

        // warna bar berubah sesuai level energi
        if (energyBar != null)
        {
            Image fill = energyBar.fillRect.GetComponent<Image>();
            if (fill != null)
            {
                if (normalizedValue > 0.5f)
                    fill.color = Color.green;
                else if (normalizedValue > 0.2f)
                    fill.color = Color.yellow;
                else
                    fill.color = Color.red;
            }
        }
    }

    // Panel Control
    void HideAllPanels()
    {
        if (hudPanel != null) hudPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (levelClearPanel != null) levelClearPanel.SetActive(false);
        if (finalClearPanel != null) finalClearPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowHUD()
    {
        HideAllPanels();
        if (hudPanel != null) hudPanel.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        HideAllPanels();
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ShowLevelClear()
    {
        HideAllPanels();
        // cek level aktif — Level 2 pakai FinalClear
        int currentBuild = UnityEngine.SceneManagement.SceneManager
            .GetActiveScene().buildIndex;

        if (currentBuild == 3) // Level2 = build index 3
        {
            if (finalClearPanel != null) finalClearPanel.SetActive(true);
        }
        else
        {
            if (levelClearPanel != null) levelClearPanel.SetActive(true);
        }
    }

    public void ShowGameOver()
    {
        HideAllPanels();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }
}