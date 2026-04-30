using UnityEngine;
using System.Collections;

public class PlayerInvincibility : MonoBehaviour, IInvincibility
{
    private bool dashInvincible;
    private bool hitInvincible;
    private float hitInvincibleTimer;
    public bool IsInvincible => dashInvincible || hitInvincible;

    private void Update()
    {
        if (!hitInvincible) return;

        hitInvincibleTimer -= Time.deltaTime;

        if (hitInvincibleTimer <= 0f)
        {
            hitInvincible = false;
            hitInvincibleTimer = 0f;
        }
    }

    public void SetDashInvincible(bool value)
    { 
        dashInvincible = value;
    }

    public void StartHitInvincible(float duration)
    {
        hitInvincible = true;
        hitInvincibleTimer = duration;
    }
}
