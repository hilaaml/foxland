using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer flagRenderer;
    public Sprite flagActive;     // sprite bendera saat aktif
    public Sprite flagInactive;   // sprite bendera saat belum disentuh

    private bool _isActivated = false;

    void Start()
    {
        if (flagRenderer != null && flagInactive != null)
            flagRenderer.sprite = flagInactive;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated) return;
        if (!other.CompareTag("Player")) return;

        _isActivated = true;

        // update respawn point Finn
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
            player.SetRespawnPoint(transform);

        // ganti visual
        if (flagRenderer != null && flagActive != null)
            flagRenderer.sprite = flagActive;

        // opsional: SFX checkpoint
        // AudioManager.Instance?.PlaySFX("Checkpoint");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}