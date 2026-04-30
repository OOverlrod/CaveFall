using UnityEngine;

public class EnemyAnimatorCon : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected EnemyFSMController enemyFSMController;

    private const string IsMove = "isMove";
    private const string IsHit = "isHit";
    private const string IsDeath = "isDeath";
    private const string IsAttack = "isAttack";

    protected virtual void Awake()
    {
        if (animator == null)
            animator = GetComponentInParent<Animator>();
        if (enemyFSMController == null)
            enemyFSMController = GetComponentInParent<EnemyFSMController>();
    }
    protected virtual void Update()
    {
        if (animator == null || enemyFSMController == null)
            return;
        if (enemyFSMController.CurrentState == EnemyFSMController.EnemyState.Dead)
            return;
        bool isMove = enemyFSMController.IsActuallyMoving;
        animator.SetBool(IsMove, isMove);
    }
    public virtual void PlayHit()
    {
        if (animator == null) return;
        animator.SetTrigger(IsHit);
    }

    public virtual void PlayDeath()
    {
        if (animator == null) return;
        animator.SetTrigger(IsDeath);
    }

    public virtual void PlayAttack()
    {
        if (animator == null) return;
        animator.SetTrigger(IsAttack);
    }
}