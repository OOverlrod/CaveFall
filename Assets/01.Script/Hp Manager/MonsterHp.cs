using UnityEngine;

public class MonsterHp : HPManager
{
    [Header("Ref")]
    [SerializeField] private EnemyAnimatorCon enemyAnimatorCon;
    [SerializeField] private float destroyDelay = 0.7f;

    protected override void Awake()
    {
        base.Awake();
        if (enemyAnimatorCon == null)
            enemyAnimatorCon = GetComponentInChildren<EnemyAnimatorCon>();
    }

    public void ApplyStageHp()
    {
        if (StageProgressManager.Instance == null) return;

        int hp = StageProgressManager.Instance.GetMonsterHpByStage();
        SetMaxHealth(hp);
    }

    public override void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0) return;

        base.TakeDamage(amount);

        if (!IsDead)
        {
            if (enemyAnimatorCon != null)
            {
                enemyAnimatorCon.PlayHit();
            }
        }
    }

    protected override void Die()
    {
        if (hasDied) return;
        hasDied = true;

        if (RunStatsManager.Instance != null)
        {
            RunStatsManager.Instance.AddMonsterKill();
        }

        NotifyDied();
        TryHealPlayer();

        if (enemyAnimatorCon != null)
        {
            enemyAnimatorCon.PlayDeath();
        }

        Destroy(gameObject, destroyDelay);
    }

    private void TryHealPlayer() //몬스터 사망시 플레이어 체력 회복
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) return;

        PlayerHp playerHp = playerObj.GetComponent<PlayerHp>();
        if (playerHp == null) return;
        
        playerHp.TryHealOnKill();
    }
}
