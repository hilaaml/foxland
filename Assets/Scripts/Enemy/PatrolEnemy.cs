using UnityEngine;

public enum PatrolType
{
    Horizontal,
    Vertical,
    Stationary
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Mode")]
    public PatrolType patrolType = PatrolType.Horizontal;
    public float moveSpeed = 2f;

    [Header("Horizontal Settings")]
    public Transform edgeCheck;
    public Transform wallCheck;
    public float edgeCheckDistance = 0.5f;
    public float wallCheckDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("Vertical Settings")]
    public float verticalRange = 2f;     // jarak naik-turun dari posisi awal
    private float _topY;
    private float _bottomY;

    private bool _movingPositive = true; // arah umum: kanan (horizontal) / atas (vertical)
    private Vector3 _startPos;

    private Rigidbody2D _rb;
    private Animator _anim;

    [Header("Flip Cooldown")]
    public float flipCooldown = 0.25f;
    private float _lastFlipTime = -999f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _startPos = transform.position;

        if (patrolType == PatrolType.Vertical)
        {
            _topY = _startPos.y + verticalRange;
            _bottomY = _startPos.y - verticalRange;
        }
    }

    void FixedUpdate()
    {
        switch (patrolType)
        {
            case PatrolType.Horizontal:
                PatrolHorizontal();
                CheckTurnConditionsHorizontal();
                break;

            case PatrolType.Vertical:
                PatrolVertical();
                break;

            case PatrolType.Stationary:
                _rb.linearVelocity = Vector2.zero;
                break;
        }

        UpdateAnimation();
    }

    // Horizontal
    void PatrolHorizontal()
    {
        float direction = _movingPositive ? 1f : -1f;
        _rb.linearVelocity = new Vector2(direction * moveSpeed, _rb.linearVelocity.y);
    }

    void CheckTurnConditionsHorizontal()
    {
        if (edgeCheck == null || wallCheck == null) return;
        if (Time.time - _lastFlipTime < flipCooldown) return;

        bool groundAhead = Physics2D.Raycast(
            edgeCheck.position,
            Vector2.down,
            edgeCheckDistance,
            groundLayer);

        Vector2 wallDir = _movingPositive ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.BoxCast(
            wallCheck.position,
            new Vector2(0.05f, 0.3f),  // kotak kecil setinggi badan
            0f,
            wallDir,
            wallCheckDistance,
            groundLayer);

        bool wallAhead = wallHit.collider != null;

        if (!groundAhead || wallAhead)
        {
            FlipHorizontal();
            _lastFlipTime = Time.time;
        }
    }

    void FlipHorizontal()
    {
        _movingPositive = !_movingPositive;
        transform.localScale = new Vector3(
            -transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Vertical
    void PatrolVertical()
    {
        float direction = _movingPositive ? 1f : -1f;
        _rb.linearVelocity = new Vector2(0f, direction * moveSpeed);

        if (transform.position.y >= _topY)
            _movingPositive = false;
        else if (transform.position.y <= _bottomY)
            _movingPositive = true;
    }

    // Animation
    void UpdateAnimation()
    {
        bool isMoving = patrolType != PatrolType.Stationary;
        _anim.SetBool("IsWalking", isMoving);

        // khusus vertical, kalau sprite enemy beda arah hadap atas/bawah
        if (patrolType == PatrolType.Vertical)
            _anim.SetFloat("VerticalDirection", _movingPositive ? 1f : -1f);
    }

    // Damage
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage();
        }
    }

    // Debug Gizmos
    void OnDrawGizmosSelected()
    {
        if (patrolType == PatrolType.Horizontal)
        {
            if (edgeCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(edgeCheck.position,
                    edgeCheck.position + Vector3.down * edgeCheckDistance);
            }
            if (wallCheck != null)
            {
                Gizmos.color = Color.blue;
                Vector3 dir = _movingPositive ? Vector3.right : Vector3.left;
                Gizmos.DrawLine(wallCheck.position,
                    wallCheck.position + dir * wallCheckDistance);
            }
        }
        else if (patrolType == PatrolType.Vertical)
        {
            Gizmos.color = Color.cyan;
            Vector3 pos = Application.isPlaying ? _startPos : transform.position;
            Gizmos.DrawLine(
                new Vector3(pos.x, pos.y + verticalRange, pos.z),
                new Vector3(pos.x, pos.y - verticalRange, pos.z));
        }
    }
}