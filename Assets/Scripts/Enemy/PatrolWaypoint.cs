using UnityEngine;

public class PatrolWaypoint : MonoBehaviour
{
    private Vector3 _worldPosition;

    void Start()
    {
        // simpan posisi world saat spawn
        _worldPosition = transform.position;
    }

    void LateUpdate()
    {
        // paksa posisi tetap di world space meski parent di-flip
        transform.position = _worldPosition;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}