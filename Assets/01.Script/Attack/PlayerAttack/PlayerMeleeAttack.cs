using UnityEngine;

public class PlayerMeleeAttack : MeleeAttackBase
{
    [Header("Ref")]
    [SerializeField] private PlayerMove2D playerMove2D;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private Animator verticalAnimator;

    private void Awake()
    {
        if (playerMove2D == null) playerMove2D = GetComponentInParent<PlayerMove2D>();
        if (playerInputReader == null) playerInputReader = GetComponentInParent<PlayerInputReader>();
        if (verticalAnimator == null) Debug.LogWarning("종 스크롤 애니메이터 연결 안 됨");
    }

    private void Update()
    {
        if (playerMove2D == null || playerInputReader == null) return;

        if (playerMove2D.CurrentMode != PlayerMove2D.PlayerMode.Vertically) return;

        if (playerInputReader.AttackPressedThisFrame) 
        {
            bool didAttack = TryAttack();

            if (didAttack && verticalAnimator != null)
            {
                verticalAnimator.SetTrigger("isAttack");
            }
        }
    }

    protected override Vector2 GetAttackDirection()
    {
        if (playerMove2D == null || playerInputReader == null)
            return Vector2.zero;

        Vector2 move = playerInputReader.MoveVector;

        if (!playerMove2D.IsGrounded)
            return Vector2.down;

        if (move.y < -0.1f)
        {
            return Vector2.down;
        }

        if (move.x < -0.1f)
        {
            return Vector2.left;
        }

        if (move.x > 0.1f)
        {
            return Vector2.right;
        }

        if (playerMove2D.FacingDirection == DIRECTION.Left)
            return Vector2.left;

        return Vector2.right;
        //switch (playerMove2D.CurrentDirection)
        //{
        //    case DIRECTION.Left: return Vector2.left;
        //    case DIRECTION.Right: return Vector2.right;
        //    default: return Vector2.zero;
        //}
    }
}
