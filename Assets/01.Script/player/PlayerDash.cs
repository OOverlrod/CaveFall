using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private PlayerMove2D playerMove2D;
    [SerializeField] private Animator sideAnimator;
    [SerializeField] private PlayerInvincibility playerInvincibility;

    [Header("Dash")]
    [SerializeField] private float dashPower = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Invincible / Collision")]
    [SerializeField] private bool useInvincibleDuringDash = true;
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string enemyLayerName = "Enemy";

    private bool canDash = true;
    private bool isDashing = false;

    public bool IsDashing => isDashing;

    private int playerLayer;
    private int enemyLayer;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerInputReader == null) playerInputReader = GetComponent<PlayerInputReader>();
        if (playerMove2D == null) playerMove2D = GetComponent<PlayerMove2D>();

        playerLayer = LayerMask.NameToLayer(playerLayerName);
        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
    }

    private void Update()
    {
        if (playerMove2D == null || playerInputReader == null || rb == null) return;
        if (playerMove2D.CurrentMode == PlayerMove2D.PlayerMode.Vertically) return;
        HandleDashInput();
    }

    private void HandleDashInput()
    {
        if (playerInputReader == null || playerMove2D == null) return;
        if (!canDash || isDashing) return;

        // 횡 모드에서만 대시 허용
        if (playerMove2D.CurrentMode != PlayerMove2D.PlayerMode.Side)
            return;

        if (playerInputReader.DashPressedThisFrame)
        { StartCoroutine(DashCoroutine()); }
    }
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        if (useInvincibleDuringDash && playerInvincibility != null)
        { playerInvincibility.SetDashInvincible(true); }

        if (playerLayer != -1 && enemyLayer != -1)// 적과 충돌 무시
        { Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true); }

        if (sideAnimator != null)// 애니메이션
        { sideAnimator.SetTrigger("isDash"); }

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDirection = 1f;

        if (playerMove2D.CurrentDirection == DIRECTION.Left)
        { dashDirection = -1f; }
        else if (playerMove2D.CurrentDirection == DIRECTION.Right)
        { dashDirection = 1f; }

        rb.linearVelocity = new Vector2(dashDirection * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        if (useInvincibleDuringDash && playerInvincibility != null)
        { playerInvincibility.SetDashInvincible(false); }
       
        if (playerLayer != -1 && enemyLayer != -1) // 적과 충돌 복구
        { Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false); }
        

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}