using System;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 5;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected Slider hpUI;

    [Header("Sound Effects")]
    [SerializeField] protected AudioClip hitSfx;
    [SerializeField] protected float hitSfxVolume = 0.3f;

    public bool setFullHealthOnStart = true;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public float HealthPercentage => (float)currentHealth / maxHealth;
    public bool IsDead => currentHealth <= 0;

    protected bool hasDied = false;

    public event Action OnDamaged;
    public event Action OnDied;

    protected virtual void Awake()
    {
        if (setFullHealthOnStart) FullHealth();
        UpdateHPUI();
    }

    public int FullHealth()
    {
        currentHealth = maxHealth;
        return currentHealth;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        UpdateHPUI();
    }

    public virtual void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (SoundManager.Instance != null && hitSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(hitSfx, hitSfxVolume);
        }
        UpdateHPUI();

        if (IsDead) Die();

    }

    public virtual void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHPUI();
    }
    protected void NotifyDied()
    {
        OnDied?.Invoke();
    }
    protected virtual void Die()
    {
        if (hasDied) return;
        hasDied = true;

        NotifyDied();

       Destroy(gameObject);
    }
    protected virtual void UpdateHPUI()
    {
        if (hpUI != null)
        {
            hpUI.maxValue = maxHealth;
            hpUI.value = currentHealth;
        }
    }
}
