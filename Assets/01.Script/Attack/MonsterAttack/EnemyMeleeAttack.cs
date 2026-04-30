using UnityEngine;

public class EnemyMeleeAttack : MeleeAttackBase
{
    [Header("Ref")]
    [SerializeField] private Transform target;

    public void SetTarget(Transform newtarget)
    {
        target = newtarget;
    }

    public void PlayEnemyAttackSfx()
    {
        PlayMissSfx();
    }

    public override bool TryAttack()
    {
        if (!CanAttack) return false;

        lastAttackTime = Time.time;

        PerformMeleeAttack();

        return true;
    }

    protected override Vector2 GetAttackDirection()
    {
        if (target == null)
        {
            return Vector2.right;
        }

        if (target.position.x < transform.position.x)
        {
            return Vector2.left;
        }
        return Vector2.right;
    }
}
