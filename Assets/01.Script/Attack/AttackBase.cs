using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    [Header("Common Attack")]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float cooldown = 0.2f;
    [SerializeField] protected LayerMask targetLayer;

    protected float lastAttackTime = -999f;

    public bool CanAttack => Time.time >= lastAttackTime + cooldown;

    public virtual bool TryAttack()
    {
        if (!CanAttack) return false;

        lastAttackTime = Time.time;
        PerformAttack();
        return true;
    }

    protected abstract void PerformAttack();

    protected bool ApplyDamage(Collider2D target)
    {
        if (target == null) return false;

        PlayerHp playerHp = target.GetComponent<PlayerHp>();
        if (playerHp == null)
        {
            playerHp = target.GetComponentInParent<PlayerHp>();
        }

        if (playerHp != null && !playerHp.IsDead)
        {
            playerHp.TakePlayerDamage(damage);
            return true;
        }

        HPManager hp = target.GetComponent<HPManager>();
        if (hp == null)
        {
            hp = target.GetComponentInParent<HPManager>();
        }

        if (hp != null && !hp.IsDead)
        {
            hp.TakeDamage(damage);
            return true;
        }

        return false;
    }
}
