using UnityEngine;

public class MeleeAttackBase : AttackBase
{
    [Header("방향별 히트박스 크기")]
    [Tooltip("좌우 공격일 때 사용할 히트박스 크기")]
    [SerializeField] private Vector2 horizontalSize = new Vector2(1.2f, 0.8f);
    [Tooltip("상하 공격일 때 사용할 히트박스 크기")]
    [SerializeField] private Vector2 verticalSize = new Vector2(0.8f, 1.2f);
    [SerializeField] private float attackDistance = 1f;

    [Header("Melee Sound")]
    [SerializeField] protected AudioClip missSfx;
    [SerializeField] protected float missSfxVolume = 0.5f;

    public override bool TryAttack()
    {
        if (!CanAttack) return false;

        lastAttackTime = Time.time;

        bool didHit = PerformMeleeAttack();

        if (!didHit)
        {
            PlayMissSfx();
        }

        return true;
    }

    protected override void PerformAttack()
    {
        PerformMeleeAttack();
    }

    protected bool PerformMeleeAttack()
    {
        Vector2 dir = GetAttackDirection();
        Vector2 size = GetAttackSize(dir);
        Vector2 center = (Vector2)transform.position + dir * attackDistance;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, targetLayer);

        bool didHit = false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject == gameObject) continue;

            if (ApplyDamage(hits[i]))
            {
                didHit = true;
            }
        }

        return didHit;
    }

    protected virtual void PlayMissSfx()
    {
        if (SoundManager.Instance != null && missSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(missSfx, missSfxVolume);
        }
    }

    protected virtual Vector2 GetAttackDirection()
    {
        return Vector2.right;
    }

    protected Vector2 GetAttackSize(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return horizontalSize;

        return verticalSize;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Vector2 dir = GetAttackDirection();
        Vector2 size = GetAttackSize(dir);
        Vector2 center = (Vector2)transform.position + dir * attackDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
