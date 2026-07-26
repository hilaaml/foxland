using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Lives")]
    public Image heartBarImage;
    public Sprite[] heartSprites;

    [Header("Gem Counter")]
    public TextMeshProUGUI gemCounterText;

    [Header("Energy Bar")]
    public Slider energyBar;

    public static HUDController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void UpdateLives(int currentLives)
    {
        if (heartBarImage == null || heartSprites == null) return;
        int index = Mathf.Clamp(currentLives, 0, heartSprites.Length - 1);
        heartBarImage.sprite = heartSprites[index];
    }

    public void UpdateGemCounter(int collected, int required)
    {
        if (gemCounterText != null)
            gemCounterText.text = $"{collected}/{required}";
    }

    public void UpdateEnergyBar(float normalizedValue)
    {
        if (energyBar == null) return;
        energyBar.value = normalizedValue;

        if (energyBar.fillRect == null) return;
        Image fill = energyBar.fillRect.GetComponent<Image>();
        if (fill == null) return;

        if (normalizedValue > 0.5f) fill.color = Color.green;
        else if (normalizedValue > 0.2f) fill.color = Color.yellow;
        else fill.color = Color.red;
    }
}