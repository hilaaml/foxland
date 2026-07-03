using UnityEngine;
using System.Collections;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy Settings")]
    [Range(0, 100)] public float maxEnergy = 100f;
    public float currentEnergy;

    // drain 1% setiap 3 detik
    public float drainRate = 1f;
    public float drainInterval = 3f;

    [Header("Speed Penalty")]
    public float lowSpeedMultiplier = 0.5f; // kecepatan saat energi 0

    [Header("Hunger")]
    public float hungerTimeout = 30f; // 30 detik di 0% → -1 life
    private float _hungerTimer = 0f;
    private bool _isHungry = false;

    [Header("Cherry Restore")]
    public float cherryRestoreAmount = 25f;

    private PlayerController _controller;
    private PlayerHealth _health;
    private bool _isRunning = true;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _health = GetComponent<PlayerHealth>();
        currentEnergy = maxEnergy;
    }

    void Start()
    {
        StartCoroutine(DrainRoutine());
        UIManager.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
    }

    IEnumerator DrainRoutine()
    {
        while (_isRunning)
        {
            yield return new WaitForSeconds(drainInterval);

            if (currentEnergy > 0)
            {
                currentEnergy -= drainRate;
                currentEnergy = Mathf.Max(0f, currentEnergy);
                UIManager.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
            }

            UpdateSpeedPenalty();
            HandleHunger();
        }
    }

    void HandleHunger()
    {
        if (currentEnergy <= 0)
        {
            _isHungry = true;
            _hungerTimer += drainInterval;

            if (_hungerTimer >= hungerTimeout)
            {
                _health.LoseLife();
                _hungerTimer = 0f; // reset timer, terus berulang
            }
        }
        else
        {
            // energi terisi → reset hunger
            _isHungry = false;
            _hungerTimer = 0f;
        }
    }

    void UpdateSpeedPenalty()
    {
        if (_controller == null) return;

        if (currentEnergy <= 0)
            _controller.speedMultiplier = lowSpeedMultiplier;
        else
            _controller.speedMultiplier = 1f;
    }

    public void ConsumeCherry()
    {
        currentEnergy += cherryRestoreAmount;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        _hungerTimer = 0f;
        _isHungry = false;
        UpdateSpeedPenalty();
        UIManager.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
        AudioManager.Instance?.PlaySFX("EatCherry");
    }

    public void ResetEnergy()
    {
        currentEnergy = maxEnergy;
        _hungerTimer = 0f;
        _isHungry = false;
        UpdateSpeedPenalty();
        UIManager.Instance?.UpdateEnergyBar(currentEnergy / maxEnergy);
    }

    public bool IsHungry() => _isHungry;
}