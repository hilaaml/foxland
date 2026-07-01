using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector2 offset = new Vector2(2f, 1f);

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Bounds — sesuaikan ke ukuran level")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Look Ahead — kamera sedikit melihat ke depan")]
    public float lookAheadDistance = 1.5f;
    public float lookAheadSpeed = 3f;
    private float _currentLookAhead = 0f;

    private float _lastTargetX;

    void Start()
    {
        if (target == null)
        {
            // auto-find Finn jika belum di-assign
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        if (target != null)
        {
            _lastTargetX = target.position.x;
            // snap kamera langsung ke posisi awal tanpa smooth
            SnapToTarget();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        UpdateLookAhead();
        FollowTarget();
    }

    void UpdateLookAhead()
    {
        float deltaX = target.position.x - _lastTargetX;

        if (Mathf.Abs(deltaX) > 0.01f)
        {
            float targetLookAhead = Mathf.Sign(deltaX) * lookAheadDistance;
            _currentLookAhead = Mathf.Lerp(
                _currentLookAhead,
                targetLookAhead,
                lookAheadSpeed * Time.deltaTime);
        }

        _lastTargetX = target.position.x;
    }

    void FollowTarget()
    {
        Vector3 desiredPos = new Vector3(
            target.position.x + offset.x + _currentLookAhead,
            target.position.y + offset.y,
            transform.position.z);      // pertahankan Z kamera

        // clamp dalam batas level
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);

        // smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime);
    }

    void SnapToTarget()
    {
        if (target == null) return;

        Vector3 snapPos = new Vector3(
            Mathf.Clamp(target.position.x + offset.x, minX, maxX),
            Mathf.Clamp(target.position.y + offset.y, minY, maxY),
            transform.position.z);

        transform.position = snapPos;
    }

    // visualisasi bounds di Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // gambar batas kamera sebagai rectangle
        Vector3 topLeft = new Vector3(minX, maxY, 0f);
        Vector3 topRight = new Vector3(maxX, maxY, 0f);
        Vector3 bottomLeft = new Vector3(minX, minY, 0f);
        Vector3 bottomRight = new Vector3(maxX, minY, 0f);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}