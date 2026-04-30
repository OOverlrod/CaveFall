using UnityEngine;

public class MyVerticalAnimatorCon : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerMove2D myplayerMove2D;
    [SerializeField] private PlayerInputReader playerinput;

    private void Awake()
    {
        if (myAnimator == null)
            myAnimator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        if (myAnimator == null || rb == null || playerinput == null || myplayerMove2D == null)
            return;

        Vector2 move = playerinput.MoveVector;

        //방향
        myAnimator.SetFloat("MoveX", move.x);

        bool isGroundMoving = Mathf.Abs(move.x) > 0.1f && myplayerMove2D.IsGrounded;
        myAnimator.SetBool("isMoving", isGroundMoving);

        // 상태
        myAnimator.SetBool("isFalling", rb.linearVelocity.y < -0.1f);
        myAnimator.SetBool("isGrounded", myplayerMove2D.IsGrounded);

        // 좌우 방향
        if (myplayerMove2D.CurrentDirection == DIRECTION.Right)
        {
            myAnimator.SetFloat("FacingX", 0.5f);
        }
        else if (myplayerMove2D.CurrentDirection == DIRECTION.Left)
        {
            myAnimator.SetFloat("FacingX", -0.5f);
        }

        // 공격
        if (playerinput.AttackPressedThisFrame)
        {
            bool isDownAttack = false;

            // 공중이면 무조건 아래 공격
            if (!myplayerMove2D.IsGrounded)
            {
                isDownAttack = true;
            }
            // 지상에서도 아래 입력이면 아래 공격
            else if (move.y < -0.1f)
            {
                isDownAttack = true;
            }

            myAnimator.SetBool("isDownAttack", isDownAttack);
        }
    }
}
