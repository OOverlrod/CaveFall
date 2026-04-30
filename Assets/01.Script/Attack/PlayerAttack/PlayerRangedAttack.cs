using UnityEngine;

public class PlayerRangedAttack : RangedAttackBase
{
    [Header("Ref")]
    [SerializeField] private PlayerMove2D playerMove2D;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private Animator sideAnimator;

    private bool lastFacing = true;

    private void Awake()
    {
        if (playerMove2D == null) playerMove2D = GetComponentInParent<PlayerMove2D>();
        if (playerInputReader == null) playerInputReader = GetComponentInParent<PlayerInputReader>();
        if (sideAnimator == null) Debug.LogWarning("횡 스크롤 애니메이터 연결 안 됨");
    }
    private void Update()
    {
        if (playerMove2D == null || playerInputReader == null) return;

        if (playerMove2D.CurrentMode != PlayerMove2D.PlayerMode.Side) return;

        Vector2 move = playerInputReader.MoveVector;

        if (move.x > 0.1f)
            lastFacing = true;
        else if (move.x < -0.1f)
            lastFacing = false;

        if (playerInputReader.AttackPressedThisFrame)
        {
            TryAttack();
        }
    }

    protected override void PerformAttack()
    {
        Vector2 dir = GetAttackDirection();

        if (dir == Vector2.zero)
            return;

        base.PerformAttack();
    }

    protected override Vector2 GetAttackDirection()
    {
        if (playerMove2D == null || playerInputReader == null)
            return Vector2.zero;

        Vector2 move = playerInputReader.MoveVector;

        if (move.x < -0.1f)
            return Vector2.left;

        if (move.x > 0.1f)
            return Vector2.right;

        return lastFacing ? Vector2.right : Vector2.left;
        /* 초기 적용 공격 입력 방향
        if (playerMove2D == null)
            return Vector2.zero;

        switch (playerMove2D.CurrentDirection)
        {
            case DIRECTION.Left: return Vector2.left;
            case DIRECTION.Right: return Vector2.right;
            case DIRECTION.Down: return Vector2.zero;
            case DIRECTION.Up: return Vector2.zero;
            default: return Vector2.zero;
        }
        */
    }
}
