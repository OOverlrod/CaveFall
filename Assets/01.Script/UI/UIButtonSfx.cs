using UnityEngine;

public class UIButtonSfx : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSfx;
    [SerializeField] private float volume = 0.5f;

    public void PlaySfx()
    {
        if (SoundManager.Instance != null && buttonSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(buttonSfx, volume);
        }
    }
}
