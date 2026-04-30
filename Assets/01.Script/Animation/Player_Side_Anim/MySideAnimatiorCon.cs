using UnityEngine;

public class MySideAnimatorCon : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private PlayerMove2D myplayerMove2D;
    [SerializeField] private PlayerInputReader playerinput;

    private bool lastFacing = true;

    private void Awake()
    {
        if (myAnimator == null) myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (myAnimator == null || playerinput == null || myplayerMove2D == null)
            return;

        // 좌우 방향
        Vector2 move = playerinput.MoveVector;

        bool isSideMoving = Mathf.Abs(move.x) > 0.1f;
        myAnimator.SetBool("isMoving", isSideMoving);
        myAnimator.SetFloat("MoveX", move.x);

        //점프
        myAnimator.SetBool("isGrounded", myplayerMove2D.IsGrounded);

        //공격
        if (playerinput.AttackPressedThisFrame)
            myAnimator.SetTrigger("isAttack");

        if (move.x > 0.1f)
            lastFacing = true;
        else if (move.x < -0.1f)
            lastFacing = false;

        myAnimator.SetBool("isFacing", lastFacing);
        myAnimator.SetFloat("FacingX", lastFacing ? 0.5f : -0.5f);
        /* 초기에 사용한 애니메이션 함수
        if (myplayerMove2D.CurrentDirection == DIRECTION.Left)
        {
            myAnimator.SetBool("isFacing", false);
            myAnimator.SetFloat("FacingX", -0.5f);
        }
        else if (myplayerMove2D.CurrentDirection == DIRECTION.Right)
        {
            myAnimator.SetBool("isFacing", true);
            myAnimator.SetFloat("FacingX", 0.5f);
        }
        */
    }
}
