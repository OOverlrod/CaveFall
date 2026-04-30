using UnityEngine;

public class BossHp : HPManager
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    protected override void Die()
    {
        if (hasDied) return;
        hasDied = true;

        Debug.Log($"{gameObject.name} 보스 사망");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Invoke(nameof(HandleBossDeath), 1.5f);
    }

    private void HandleBossDeath()
    {
        // 나중에 여기서 보스전 클리어 처리
        // 예: 문 열기

        Destroy(gameObject);
    }
}
