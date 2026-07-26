using UnityEngine;
using System.Collections;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy Settings")]
    [Range(0, 100)] public float maxEnergy = 100f;
    public float currentEnergy;

    public float drainRate = 1f;
    public float drainInterval = 3f;

    [Header("Speed Penalty")]
    [Tooltip("Energy threshold mulai melambat (0-100)")]
    public float slowThreshold = 25f;
    public float lowSpeedMultiplier = 0.5f;

    [Header("Hunger")]
    public float hungerTimeout = 30f;
    private float _hungerTimer = 0f;

    [Header("Cherry Restore")]
    public float cherryRestoreAmount = 25f;

    private PlayerController _controller;
    private PlayerHealth _health;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _health = GetComponent<PlayerHealth>();
        currentEnergy = maxEnergy;
    }

    void Start()
    {
        StartCoroutine(DrainRoutine());
        HUDController.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
    }

    void Update()
    {
        ApplySpeedPenalty();

        if (currentEnergy <= 0f)
        {
            _hungerTimer += Time.deltaTime;

            if (_hungerTimer >= hungerTimeout)
            {
                _hungerTimer = 0f;
                _health?.TakeDamage();
            }
        }
        else
        {
            _hungerTimer = 0f;
        }
    }

    IEnumerator DrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(drainInterval);

            if (currentEnergy > 0f)
            {
                currentEnergy -= drainRate;
                currentEnergy = Mathf.Max(0f, currentEnergy);
                HUDController.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
            }
        }
    }

    void ApplySpeedPenalty()
    {
        if (_controller == null) return;

        if (currentEnergy <= 0f)
        {
            _controller.speedMultiplier = lowSpeedMultiplier;
        }
        else if (currentEnergy < slowThreshold)
        {
            float t = currentEnergy / slowThreshold;
            _controller.speedMultiplier = Mathf.Lerp(lowSpeedMultiplier, 1f, t);
        }
        else
        {
            _controller.speedMultiplier = 1f;
        }
    }

    public void ConsumeCherry()
    {
        currentEnergy += cherryRestoreAmount;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        _hungerTimer = 0f;
        HUDController.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
        AudioManager.Instance?.PlaySFX(SFXId.EatCherry);
    }
}