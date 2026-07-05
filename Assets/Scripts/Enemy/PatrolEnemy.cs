using UnityEngine;

public enum PatrolType { Stationary, Horizontal, Vertical }

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Type")]
    public PatrolType patrolType = PatrolType.Horizontal;
    public float moveSpeed = 2f;

    [Header("Waypoints")]
    public Transform pointA;   // kiri / bawah
    public Transform pointB;   // kanan / atas

    private Rigidbody2D _rb;
    private bool _goingToB = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        switch (patrolType)
        {
            case PatrolType.Stationary:
                _rb.bodyType = RigidbodyType2D.Static;
                break;

            case PatrolType.Horizontal:
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _rb.gravityScale = 1f;
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;

            case PatrolType.Vertical:
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _rb.gravityScale = 0f;
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
        }
    }

    void FixedUpdate()
    {
        if (patrolType == PatrolType.Stationary) return;

        if (pointA == null || pointB == null)
        {
            Debug.LogWarning($"[{name}] PointA atau PointB belum di-assign!");
            return;
        }

        switch (patrolType)
        {
            case PatrolType.Horizontal: MoveHorizontal(); break;
            case PatrolType.Vertical: MoveVertical(); break;
        }
    }

    void MoveHorizontal()
    {
        float target = _goingToB ? pointB.position.x : pointA.position.x;
        float dir = target > transform.position.x ? 1f : -1f;

        _rb.linearVelocity = new Vector2(dir * moveSpeed, _rb.linearVelocity.y);

        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(s.x) * dir, s.y, s.z);

        if (Mathf.Abs(transform.position.x - target) < 0.2f)
            _goingToB = !_goingToB;
    }

    void MoveVertical()
    {
        float target = _goingToB ? pointB.position.y : pointA.position.y;
        float dir = target > transform.position.y ? 1f : -1f;

        _rb.linearVelocity = new Vector2(0f, dir * moveSpeed);

        if (Mathf.Abs(transform.position.y - target) < 0.2f)
            _goingToB = !_goingToB;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage();
    }

    void OnDrawGizmosSelected()
    {
        if (pointA == null || pointB == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointA.position, 0.2f);
        Gizmos.DrawWireSphere(pointB.position, 0.2f);
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}