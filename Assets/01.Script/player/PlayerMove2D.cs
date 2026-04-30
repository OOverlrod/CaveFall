
using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    public enum PlayerMode
    {
        Vertically,
        Side
    }

    [Header("Ref")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInputReader playerinputReader;
    [SerializeField] private PlayerDash playerDash;

    [Header("Mode")]
    [SerializeField] private PlayerMode currentMode = PlayerMode.Side;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("Jump")]
    [SerializeField] private float verticalJumpForce = 5f;
    [SerializeField] private float sideJumpForce = 8f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Fall")]
    [SerializeField] private float maxFallSpeed = 3f;

    private Vector2 inputDirection;
    private bool isGrounded;

    public PlayerMode CurrentMode => currentMode;

    [SerializeField]
    private DIRECTION currentDirection;
    public DIRECTION CurrentDirection => currentDirection;

    [SerializeField]
    private DIRECTION facingDirection = DIRECTION.Right;
    public DIRECTION FacingDirection => facingDirection;

    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerinputReader == null) playerinputReader = GetComponent<PlayerInputReader>();
        if (playerDash == null) playerDash = GetComponent<PlayerDash>();
    }

    private void Start()
    {
        currentDirection = DIRECTION.Right;
        facingDirection = DIRECTION.Right;
    }

    private void Update()
    {
        inputDirection = playerinputReader != null ? playerinputReader.MoveVector : Vector2.zero;

        CheckGround();
        UpdateDirection();
        HandleJump();
    }

    private void FixedUpdate()
    {
        if (playerDash != null && playerDash.IsDashing) return;

        Move();
        ClampFallSpeed();
    }

    private void Move()
    {
        float moveX = inputDirection.x;

        // y속도는 점프/낙하를 위해 유지
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }
    private void HandleJump()
    {
        if (playerinputReader == null) return;
        if (playerDash != null && playerDash.IsDashing) return;

        if (playerinputReader.JumpPressedThisFrame && isGrounded)
        {
            float currentJumpForce = GetCurrentJumpForce();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
        }
    }
    private void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
        }
    }
    private float GetCurrentJumpForce()
    {
        switch (currentMode)
        {
            case PlayerMode.Side:
                return sideJumpForce;

            case PlayerMode.Vertically:
            default:
                return verticalJumpForce;
        }
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void UpdateDirection()
    {
        const float deadzoneValue = 0.1f;

        if (inputDirection.sqrMagnitude < 0.01f) return;

        float x = inputDirection.x;
        float y = inputDirection.y;

        if (x > deadzoneValue)
        {
            currentDirection = DIRECTION.Right;
            facingDirection = DIRECTION.Right;
        }
        else if (x < -deadzoneValue)
        {
            currentDirection = DIRECTION.Left;
            facingDirection = DIRECTION.Left;
        }
        else
        {
            if (y > deadzoneValue) currentDirection = DIRECTION.Up;
            else if (y < -deadzoneValue) currentDirection = DIRECTION.Down;
        }
    }
    public void SetMode(PlayerMode newMode)
    {
        currentMode = newMode;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}