using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives")]
    public int currentLives;

    private const int DefaultMaxLives = 5;
    private int _maxLives = DefaultMaxLives;

    [Header("Respawn")]
    public float respawnDelay = 1f;
    public float invincibleDuration = 2f;
    private Vector3 _respawnPosition;
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
        currentLives = _maxLives;
    }

    void Start()
    {
        PlayerSpawnPoint spawnPoint = FindFirstObjectByType<PlayerSpawnPoint>();
        if (spawnPoint != null)
        {
            _respawnPosition = spawnPoint.transform.position;
        }
        else
        {
            _respawnPosition = transform.position;
            Debug.LogWarning("[PlayerHealth] SpawnPoint tidak ditemukan, pakai posisi awal Finn.");
        }
    }

    public void OnSceneReady()
    {
        _maxLives = LevelManager.Instance != null
            ? LevelManager.Instance.maxLives
            : DefaultMaxLives;

        currentLives = _maxLives;
        _isDead = false;
        _isInvincible = false;

        HUDController.Instance?.SetupHearts(_maxLives);
        UIManager.Instance?.UpdateLives(currentLives);
    }

    // Damage
    public void TakeDamage()
    {
        if (_isInvincible || _isDead) return;

        currentLives--;
        AudioManager.Instance?.PlaySFX(SFXId.Hurt);
        UIManager.Instance?.UpdateLives(currentLives);

        if (currentLives <= 0)
            StartCoroutine(DieRoutine());
        else
            StartCoroutine(HurtRoutine());
    }

    // Routines
    IEnumerator HurtRoutine()
    {
        _isInvincible = true;
        _controller.enabled = false;
        _rb.linearVelocity = Vector2.zero;

        _anim.SetTrigger("Hurt");

        yield return new WaitForSeconds(respawnDelay);

        // teleport ke respawn point
        transform.position = _respawnPosition;
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
        AudioManager.Instance?.PlaySFX(SFXId.Death);

        yield return new WaitForSeconds(1.5f);

        GameManager.Instance?.GameOver();
    }

    // Checkpoint
    public void SetRespawnPoint(Vector3 position)
    {
        _respawnPosition = position;
    }
}