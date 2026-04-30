using UnityEngine;

public class RangedAttackBase : AttackBase
{
    [Header("Ranged")]
    [SerializeField] private Projectile2D projectilePrefab;
    [SerializeField] private float spawnDistance = 0.8f;
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float spreadAngle = 0f;

    [Header("Ranged Sound")]
    [SerializeField] protected AudioClip fireSfx;
    [SerializeField] protected float fireSfxVolume = 0.5f;

    public override bool TryAttack()
    {
        if (!CanAttack) return false;

        lastAttackTime = Time.time;

        PlayFireSfx();
        PerformAttack();
        return true;
    }

    protected override void PerformAttack()
    {
        if (projectilePrefab == null) return;

        Vector2 dir = GetAttackDirection();

        if (projectileCount <= 1)
            FireSingle(dir);//총알이 1발일때
        else
            FireSpread(dir);//총알이 2발 이상일때
    }

    protected virtual Vector2 GetAttackDirection()
    {
        return  Vector2.right;
    }

    private void FireSingle(Vector2 dir) //총알이 1발일때
    {
        Vector3 spawnPos = transform.position + (Vector3)(dir * spawnDistance);
        Projectile2D spawnedHitBox = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        spawnedHitBox.Initialize(dir, damage, targetLayer, gameObject);
    }

    private void FireSpread(Vector2 dir) //총알이 2발 이상일때
    {
        float startAngle = -spreadAngle * 0.5f;
        float step = spreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = startAngle + step * i;
            Vector2 rotatedDir = Quaternion.Euler(0, 0, currentAngle) * dir;

            Vector3 spawnPos = transform.position + (Vector3)(rotatedDir * spawnDistance);
            Projectile2D spawnedHitBox = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            spawnedHitBox.Initialize(rotatedDir, damage, targetLayer, gameObject);
        }
    }

    protected virtual void PlayFireSfx()
    {
        if (SoundManager.Instance != null && fireSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(fireSfx, fireSfxVolume);
        }
    }
}

