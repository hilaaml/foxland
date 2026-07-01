using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives")]
    public int maxLives = 5;
    public int currentLives;

    [Header("Respawn")]
    public float respawnDelay = 1.5f;
    public Transform respawnPoint;

    [Header("Invincibility")]
    public float invincibleDuration = 1.5f;
    private bool _isInvincible = false;
    private bool _isDead = false;

    private PlayerController _controller;
    private Animator _anim;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _anim = GetComponent<Animator>();
        currentLives = maxLives;
    }

    void Start()
    {
        respawnPoint = transform;
        UIManager.Instance?.UpdateLives(currentLives);
    }

    public void TakeDamage()
    {
        if (_isInvincible || _isDead) return;

        currentLives--;
        AudioManager.Instance?.PlaySFX("Hurt");
        UIManager.Instance?.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            StartCoroutine(DieRoutine());
        }
        else
        {
            StartCoroutine(HurtRoutine());
        }
    }

    // dipanggil juga dari EnergySystem saat kelaparan
    public void LoseLife()
    {
        TakeDamage();
    }

    IEnumerator HurtRoutine()
    {
        _isInvincible = true;
        _anim.SetTrigger("Hurt");

        yield return new WaitForSeconds(respawnDelay);

        // respawn ke checkpoint atau awal level
        transform.position = respawnPoint.position;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(invincibleDuration - respawnDelay);
        _isInvincible = false;
    }

    IEnumerator DieRoutine()
    {
        _isDead = true;
        _anim.SetTrigger("Death");
        AudioManager.Instance?.PlaySFX("Death");

        // matikan kontrol
        _controller.enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1.5f);

        GameManager.Instance?.GameOver();
    }

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    public void ResetHealth()
    {
        currentLives = maxLives;
        _isDead = false;
        _isInvincible = false;
        _controller.enabled = true;
        UIManager.Instance?.UpdateLives(currentLives);
    }
}