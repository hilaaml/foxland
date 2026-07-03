using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform levelSpawnPoint;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage();
        }
    }
}