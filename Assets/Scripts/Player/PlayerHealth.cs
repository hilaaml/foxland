using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives")]
    public int maxLives = 5;
    public int currentLives;

    [Header("Respawn")]
    public float respawnDelay = 1f;
    public float invincibleDuration = 2f;
    private Transform _respawnPoint;
    private bool _isInvincible = false;
    private bool _isDead = false;

    private PlayerController _controller;
    private Animator _anim;
    private Rigidbody2D _rb;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        currentLives = maxLives;
    }

    void Start()
    {
        PlayerSpawnPoint spawnPoint = FindFirstObjectByType<PlayerSpawnPoint>();
        if (spawnPoint != null)
            _respawnPoint = spawnPoint.transform;
        else
        {
            _respawnPoint = new GameObject("FallbackSpawn").transform;
            _respawnPoint.position = transform.position;
            Debug.LogWarning("[PlayerHealth] SpawnPoint tidak ditemukan, pakai posisi awal Finn.");
        }

        UIManager.Instance?.UpdateLives(currentLives);
    }

    // Damage
    public void TakeDamage()
    {
        if (_isInvincible || _isDead) return;

        currentLives--;
        AudioManager.Instance?.PlaySFX("Hurt");
        UIManager.Instance?.UpdateLives(currentLives);

        if (currentLives <= 0)
            StartCoroutine(DieRoutine());
        else
            StartCoroutine(HurtRoutine());
    }

    public void LoseLife() => TakeDamage();

    // Routines
    IEnumerator HurtRoutine()
    {
        _isInvincible = true;
        _controller.enabled = false;
        _rb.linearVelocity = Vector2.zero;

        _anim.SetTrigger("Hurt");

        yield return new WaitForSeconds(respawnDelay);

        // teleport ke respawn point
        transform.position = _respawnPoint.position;
        _rb.linearVelocity = Vector2.zero;

        _controller.enabled = true;

        // invincibility frames setelah respawn
        yield return new WaitForSeconds(invincibleDuration);
        _isInvincible = false;
    }

    IEnumerator DieRoutine()
    {
        _isDead = true;
        _controller.enabled = false;
        _rb.linearVelocity = Vector2.zero;

        _anim.SetTrigger("Death");
        AudioManager.Instance?.PlaySFX("Death");

        yield return new WaitForSeconds(1.5f);

        GameManager.Instance?.GameOver();
    }

    // Checkpoint
    public void SetRespawnPoint(Transform point)
    {
        _respawnPoint = point;
    }

    // Reset
    public void ResetHealth()
    {
        currentLives = maxLives;
        _isDead = false;
        _isInvincible = false;
        _controller.enabled = true;
        _rb.linearVelocity = Vector2.zero;
        UIManager.Instance?.UpdateLives(currentLives);
    }
}