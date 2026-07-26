using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : Singleton<PlayerController>
{
    // Player selalu spawn baru tiap scene, jadi tidak boleh persist.
    protected override bool Persistent => false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.1f);
    public LayerMask groundLayer;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;
    private bool _wasGrounded;
    private float _moveInput;
    private bool _facingRight = true;

    [HideInInspector] public float speedMultiplier = 1f;

    protected override void Awake()
    {
        base.Awake();
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
        _wasGrounded = _isGrounded;
        CheckGround();
        DetectLanding();
        Move();
        BetterJump();
    }

    void DetectLanding()
    {
        if (!_wasGrounded && _isGrounded)
            AudioManager.Instance?.PlaySFX(SFXId.Land);
    }

    void GatherInput()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space)
                        || Input.GetKeyDown(KeyCode.W)
                        || Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpPressed && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _anim.SetTrigger("Jump");
            AudioManager.Instance?.PlaySFX(SFXId.Jump);
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

    void BetterJump()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rb.linearVelocity.y > 0)
        {
            bool jumpHeld = Input.GetKey(KeyCode.Space)
                         || Input.GetKey(KeyCode.W)
                         || Input.GetKey(KeyCode.UpArrow);

            if (!jumpHeld)
                _rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                    * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
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
    }

    public bool IsGrounded() => _isGrounded;

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}