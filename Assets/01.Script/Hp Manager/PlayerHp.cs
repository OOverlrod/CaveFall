using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHp : HPManager
{
    [Header("Animation")]
    [SerializeField] private Animator Sideanimator;
    [SerializeField] private Animator Verticallyanimator;

    [Header("Hit Effect")]
    [SerializeField] private PlayerInvincibility playerInvincibility;//피격시 무적 스크립트
    [SerializeField] private PlayerHitFlash playerHitFlash;// 피격시 깜빡이 스크립트
    [SerializeField] private float hitInvincibleDuration = 0.5f;//무적 시간

    [Header("Heal of Kill")] // 몬스터 처치시 체력 회복
    [SerializeField] private float healChance = 0.15f;// 기본 체력 회복 확률
    [SerializeField] private float bonusHealChance = 0f;// 업그레이드시 플레이어 보너스 채력 회복 확률 조정
    [SerializeField] private int healAmount = 1;// 기본 체력 회복량
    [SerializeField] private int bonusAmount = 2;// 업그레이드시 플레이어 체력 회복량 

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private float deathVolume = 0.8f;

    protected override void Awake()
    {
        base.Awake();

        if (playerInvincibility == null)
            playerInvincibility = GetComponent<PlayerInvincibility>();

        if (playerHitFlash == null)
            playerHitFlash = GetComponent<PlayerHitFlash>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        FindPlayerHpUI();
        UpdateHPUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hpUI = null;
        FindPlayerHpUI();
        UpdateHPUI();
    }

    private void FindPlayerHpUI()
    {
        if (hpUI != null) return;

        GameObject hpObject = GameObject.Find("PlayerHpUI");
        if (hpObject != null)
        {
            hpUI = hpObject.GetComponent<Slider>();
        }

        if (hpUI == null)
        {
            Debug.LogWarning("플레이어 HP UI Slider를 찾지 못했습니다.");
        }
    }

    public void TakePlayerDamage(int amount)
    {
        if (IsDead) return;

        if (playerInvincibility != null && playerInvincibility.IsInvincible)
            return;

        TakeDamage(amount);

        if (IsDead) return;

        if (playerInvincibility != null)
            playerInvincibility.StartHitInvincible(hitInvincibleDuration);

        if (playerHitFlash != null)
            playerHitFlash.PlayFlash(hitInvincibleDuration);
    }

    public void TryHealOnKill()
    {
        if (IsDead) return;

        float roll = Random.value;

        if (roll > healChance)
            return;

        int finalHeal = healAmount;

        if (Random.value < bonusHealChance)
        {
            finalHeal = bonusAmount;
        }
        Heal(finalHeal);
        Debug.Log($"플레이어 체력회복 + {finalHeal}");

    }

    protected override void Die()
    {
        if (hasDied) return;
        hasDied = true;

        if (SoundManager.Instance != null && deathSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(deathSfx, deathVolume);
        }

        PlayerMove2D move = GetComponent<PlayerMove2D>();
        if (move != null)
        {
            move.enabled = false;
        }

        PlayerInputReader inputReader = GetComponent<PlayerInputReader>();
        if (inputReader != null)
        {
            inputReader.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (move.CurrentMode == PlayerMove2D.PlayerMode.Side && Sideanimator != null)
        {
            Sideanimator.SetTrigger("isDeath");
        }
        else if (move.CurrentMode == PlayerMove2D.PlayerMode.Vertically && Verticallyanimator != null)
        {
            Verticallyanimator.SetTrigger("isDeath");
        }

        Invoke(nameof(GoToResult), 2f);
    }

    private void GoToResult()
    {

        if (RunStatsManager.Instance != null && StageProgressManager.Instance != null)
        {
            RunStatsManager.Instance.SetReachedStage(StageProgressManager.Instance.CurrentLoopLevel);
        }

        if (StageProgressManager.Instance != null)
        {
            StageProgressManager.Instance.ResetLoopLevel();
        }

        if (DontDestroyPlayer.Instance != null)
        {
            Destroy(DontDestroyPlayer.Instance.gameObject);
        }

        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.LoadSceneByName("Game Over Screen");
        }
    }
}
