using UnityEngine;

public class PatrolWaypoint : MonoBehaviour
{
    private Vector3 _worldPosition;

    void Start()
    {
        _worldPosition = transform.position;
    }

    void LateUpdate()
    {
        transform.position = _worldPosition;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}