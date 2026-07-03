using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.1f);
    public LayerMask groundLayer;

    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;
    private float _moveInput;
    private bool _facingRight = true;

    [HideInInspector] public float speedMultiplier = 1f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        GatherInput();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    void GatherInput()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _anim.SetTrigger("Jump");
            AudioManager.Instance?.PlaySFX("Jump");
        }
    }

    void Move()
    {
        float targetSpeed = _moveInput * moveSpeed * speedMultiplier;
        _rb.linearVelocity = new Vector2(targetSpeed, _rb.linearVelocity.y);

        if (_moveInput > 0 && !_facingRight) Flip();
        else if (_moveInput < 0 && _facingRight) Flip();
    }

    void CheckGround()
    {
        _isGrounded = Physics2D.OverlapBox(
            groundCheck.position, groundCheckSize, 0f, groundLayer);
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z);
    }

    void UpdateAnimations()
    {
        _anim.SetBool("IsRunning", Mathf.Abs(_moveInput) > 0.01f);
        _anim.SetBool("IsGrounded", _isGrounded);
        _anim.SetFloat("VelocityY", _rb.linearVelocity.y);
    }

    public bool IsGrounded() => _isGrounded;

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}