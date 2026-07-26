using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    void Start()
    {
        PlayerController.Instance?.GetComponent<PlayerHealth>()?.SetRespawnPoint(transform.position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawIcon(transform.position, "sv_icon_dot3_pix16_gizmo", true);
    }
}