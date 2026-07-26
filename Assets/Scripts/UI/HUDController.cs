using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HUDController : Singleton<HUDController>
{
    [Header("Hearts")]
    [Tooltip("Sprite hati penuh.")]
    public Sprite heartFullSprite;
    [Tooltip("Sprite hati kosong.")]
    public Sprite heartEmptySprite;
    [Tooltip("Kontainer hati sejumlah maxlife.")]
    public Transform heartsContainer;
    [Tooltip("Ukuran tiap icon heart (width, height) dalam pixel UI.")]
    public Vector2 heartIconSize = new Vector2(40f, 40f);

    [Header("Editor Preview Heart")]
    public int previewMaxLives = 5;

    [Header("Gem Counter")]
    public TextMeshProUGUI gemCounterText;

    [Header("Energy Bar")]
    public Slider energyBar;

    protected override bool Persistent => false;

    private readonly List<Image> _heartIcons = new List<Image>();

    public void SetupHearts(int maxLives)
    {
        if (heartsContainer == null)
        {
            Debug.LogWarning("[HUDController] heartsContainer belum di-assign di Inspector.");
            return;
        }

        for (int i = heartsContainer.childCount - 1; i >= 0; i--)
            Destroy(heartsContainer.GetChild(i).gameObject);
        _heartIcons.Clear();

        for (int i = 0; i < maxLives; i++)
        {
            GameObject go = new GameObject($"Heart_{i}", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(heartsContainer, false);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = heartIconSize;

            Image img = go.GetComponent<Image>();
            img.sprite = heartFullSprite;

            _heartIcons.Add(img);
        }

        UpdateLives(maxLives);
    }

    public void UpdateLives(int currentLives)
    {
        if (heartFullSprite == null || heartEmptySprite == null) return;

        for (int i = 0; i < _heartIcons.Count; i++)
            _heartIcons[i].sprite = i < currentLives ? heartFullSprite : heartEmptySprite;
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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (heartsContainer == null) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = heartsContainer.localToWorldMatrix;

        Vector3 size = new Vector3(heartIconSize.x, heartIconSize.y, 0f);

        for (int i = 0; i < previewMaxLives; i++)
        {
            Vector3 localPos = new Vector3(i * heartIconSize.x, 0f, 0f);
            Gizmos.DrawWireCube(localPos, size);
        }

        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}