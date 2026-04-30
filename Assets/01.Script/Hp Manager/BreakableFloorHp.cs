using UnityEngine;

public class BreakableFloorHp : HPManager
{
    [Header("Break Effect")]
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private AudioClip breakSfx;
    [SerializeField] private float breakSfxVolume = 0.5f;

    protected override void Die()
    {
        if (hasDied) return;
        hasDied = true;

        if (SoundManager.Instance != null && breakSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(breakSfx, breakSfxVolume);
        }

        RunStatsManager.Instance?.AddDestroyedFloor();

        Destroy(gameObject);
    }
}
