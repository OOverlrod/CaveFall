using System;
using UnityEngine;

public class EnemyFSMController : MonoBehaviour
{
    public enum MoveType
    {
        Flying,
        Ground
    }

    public enum EnemyState
    {
        Idle,
        Chase,
        AttackWindup,
        Attack,
        Dead
    }

    [Header("Ref")]
    [SerializeField] private HPManager hpManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyMeleeAttack enemyMeleeAttack;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnemyAnimatorCon enemyAnimatorCon;

    [Header("FSM")]
    [SerializeField] private EnemyState currentState = EnemyState.Idle;
    [SerializeField] private float chaseDistance = 5f;
    [SerializeField] private float attackDistance = 1.25f;
    [SerializeField] private float stopDistance = 0.8f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseExitMultiplier = 1.2f;

    [Header("Attack Timing")]
    [SerializeField] private float attackDelay = 0.25f;

    [Header("Mode")]
    [SerializeField] private MoveType moveType = MoveType.Flying;

    [Header("Ground Check")]//추후 수정 사항
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    public event Action OnEnemyDefeated;
    public event Action<EnemyState> OnEnemyStateChanged;

    public EnemyState CurrentState => currentState;

    private float attackStartTime;

    private bool isActuallyMoving;
    public bool IsActuallyMoving => isActuallyMoving;

    private void Awake()
    {
        if (hpManager == null)
            hpManager = GetComponentInChildren<HPManager>();

        if (rb == null)
            rb = GetComponentInChildren<Rigidbody2D>();

        if (enemyMeleeAttack == null)
            enemyMeleeAttack = GetComponentInChildren<EnemyMeleeAttack>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (enemyAnimatorCon == null)
            enemyAnimatorCon = GetComponentInChildren<EnemyAnimatorCon>();
    }

    private void OnEnable()
    {
        if (hpManager != null)
        {
            hpManager.OnDied += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (hpManager != null)
        {
            hpManager.OnDied -= HandleDeath;
        }
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead)
            return;

        if (target == null)
            TryFindTarget();

        if (target == null)
            return;

        UpdateFacing();

        if (currentState == EnemyState.AttackWindup)
        {
            UpdateAttackWindup();
            return;
        }

        EvaluateStateTransition();
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyState.Dead)
            return;

        if (currentState == EnemyState.AttackWindup || currentState == EnemyState.Attack)
        {
            StopMovement();

            if (currentState == EnemyState.Attack)
            {
                TryAttackTarget();
            }
            return;
        }

        if (currentState == EnemyState.Chase)
        {
            HandleChaseMovement();
            return;
        }
    }

    private void TryFindTarget()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;

            if (enemyMeleeAttack != null)
            {
                enemyMeleeAttack.SetTarget(target);
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (enemyMeleeAttack != null)
        {
            enemyMeleeAttack.SetTarget(target);
        }
    }

    private void EvaluateStateTransition()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToTarget <= chaseDistance)
                    TransitionTo(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                if (distanceToTarget <= attackDistance)
                {
                    if (enemyMeleeAttack != null && enemyMeleeAttack.CanAttack)
                    {
                        TransitionTo(EnemyState.AttackWindup);
                    }
                }
                else if (distanceToTarget > chaseDistance * chaseExitMultiplier)
                {
                    TransitionTo(EnemyState.Idle);
                }
                break;

            case EnemyState.Attack:
                if (distanceToTarget > attackDistance)
                    TransitionTo(EnemyState.Chase);
                break;
        }
    }

    private void TransitionTo(EnemyState nextState)
    {
        if (currentState == nextState)
            return;

        currentState = nextState;

        if (currentState == EnemyState.Idle)
        {
            isActuallyMoving = false;
            StopMovement();
        }

        if (currentState == EnemyState.AttackWindup)
        {
            attackStartTime = Time.time;

            if (enemyAnimatorCon != null)
            {
                enemyAnimatorCon.PlayAttack();
            }

            if (enemyMeleeAttack != null)
            {
                enemyMeleeAttack.PlayEnemyAttackSfx();
            }
        }

        OnEnemyStateChanged?.Invoke(currentState);
    }

    private void HandleChaseMovement()
    {
        if (target == null || rb == null)
            return;
        if (moveType == MoveType.Flying)
        {
            Vector2 toTarget = (Vector2)target.position - rb.position;
            float distance = toTarget.magnitude;

            if (distance <= stopDistance)
            {
                isActuallyMoving = false;
                return;
            }

            Vector2 direction = toTarget.normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            isActuallyMoving = true;
        }
        else if (moveType == MoveType.Ground)
        {
            if (!IsGrounded())
            {
                isActuallyMoving = false;
                return;
            }

            float dirX = target.position.x - transform.position.x;

            float absX = Mathf.Abs(dirX);

            if (absX <= stopDistance)
            {
                isActuallyMoving = false;
                return;
            }

            if (absX < 0.05f)
            {
                isActuallyMoving = false;
                return;
            }

            float moveX = Mathf.Sign(dirX);

            Vector2 nextPos = rb.position + new Vector2(moveX * moveSpeed * Time.fixedDeltaTime, 0f);
            rb.MovePosition(nextPos);
            isActuallyMoving = true;
        }
    }

    private void UpdateAttackWindup()
    {
        if (target == null)
        {
            TransitionTo(EnemyState.Idle);
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        
        if (distanceToTarget > attackDistance)// 선딜 중에 범위 밖으로 나가면 공격 취소
        {
            TransitionTo(EnemyState.Chase);
            return;
        }
        
        if (Time.time >= attackStartTime + attackDelay)// 선딜 끝나면 실제 공격 상태로
        {
            TransitionTo(EnemyState.Attack);
        }
    }

    private void TryAttackTarget()
    {
        if (enemyMeleeAttack == null || target == null)
            return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget > attackDistance)
        {
            TransitionTo(EnemyState.Chase);
            return;
        }

        bool didAttack = enemyMeleeAttack.TryAttack();

        if (didAttack)
        {
            TransitionTo(EnemyState.Chase);
        }
    }

    private void UpdateFacing()
    {
        if (target == null || spriteRenderer == null)
            return;

        spriteRenderer.flipX = target.position.x < transform.position.x;
    }

    private void HandleDeath()
    {
        TransitionTo(EnemyState.Dead);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (enemyAnimatorCon != null)
        {
            enemyAnimatorCon.PlayDeath();
        }

        OnEnemyDefeated?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance * chaseExitMultiplier);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        if (target != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, target.position);
        }

        if (moveType == MoveType.Flying )return;
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private void StopMovement()
    {
        if (rb == null)
            return;

        rb.linearVelocity = Vector2.zero;
        rb.MovePosition(rb.position);
        isActuallyMoving = false;
    }
}