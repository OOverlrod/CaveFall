using UnityEngine;

public class PlayerHitFlash : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [Header("Flash Settings")]
    [SerializeField] private float flashInterval = 0.08f;
    [SerializeField] private float transparentAlpha = 0.35f;

    private float timer;
    private float duration;
    private bool isTransparent;
    private bool isFlashing;

    private void Awake()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        }
    }

    private void Update()
    {
        if (!isFlashing) return;

        duration -= Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= flashInterval)
        {
            timer = 0f;

            SetAlpha(isTransparent ? 1f : transparentAlpha);
            isTransparent = !isTransparent;
        }

        if (duration <= 0f)
        {
            StopFlash();
        }
    }

    public void PlayFlash(float flashDuration)
    {
        isFlashing = true;
        duration = flashDuration;
        timer = 0f;
        isTransparent = false;
        SetAlpha(1f);
    }

    private void StopFlash()
    {
        isFlashing = false;
        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] == null) continue;

            Color color = spriteRenderers[i].color;
            color.a = alpha;
            spriteRenderers[i].color = color;
        }
    }
}