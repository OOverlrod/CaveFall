using UnityEngine;

[DisallowMultipleComponent]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("AudioSource (Optional)")]
    [Tooltip("비어 있으면 Awake에서 자동 생성한다. BGM 전용 AudioSource.")]
    [SerializeField] private AudioSource bgmSource;
    [Tooltip("비어 있으면 Awake에서 자동 생성한다. SFX 전용 AudioSource.")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    public float MasterVolume => masterVolume;
    public float BgmVolume => bgmVolume;
    public float SfxVolume => sfxVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        EnsureAudioSources();
    }

    public void PlayBgm(AudioClip bgmClip, float volume = 1f, bool loop = true)
    {
        if (bgmClip == null)
        {
            Debug.LogWarning("[SoundManager] PlayBgm 실패: bgmClip이 null입니다.");
            return;
        }

        if (bgmSource == null)
        {
            Debug.LogWarning("[SoundManager] PlayBgm 실패: bgmSource가 없습니다.");
            return;
        }

        bgmSource.clip = bgmClip;
        bgmSource.loop = loop;
        bgmSource.volume = Mathf.Clamp01(volume * masterVolume * bgmVolume);
        bgmSource.Play();
    }

    public void StopBgm()
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("[SoundManager] StopBgm 실패: bgmSource가 없습니다.");
            return;
        }

        bgmSource.Stop();
    }

    public void PlaySfxOneShot(AudioClip sfxClip, float volumeScale = 1f)
    {
        if (sfxClip == null)
        {
            Debug.LogWarning("[SoundManager] PlaySfxOneShot 실패: sfxClip이 null입니다.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("[SoundManager] PlaySfxOneShot 실패: sfxSource가 없습니다.");
            return;
        }

        sfxSource.PlayOneShot(sfxClip, Mathf.Clamp01(volumeScale * masterVolume * sfxVolume));
    }

    private void EnsureAudioSources()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value / 10f);
        ApplyVolume();
        //masterVolume = Mathf.Clamp01(value);

        //if (bgmSource != null)
        //{
        //    bgmSource.volume = masterVolume * bgmVolume;
        //}
    }

    public void SetBgmVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value / 10f);
        ApplyVolume();
        //bgmVolume = Mathf.Clamp01(value);

        //if (bgmSource != null)
        //{
        //    bgmSource.volume = masterVolume * bgmVolume;
        //}
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value / 10f);
        //sfxVolume = Mathf.Clamp01(value);
    }

    private void ApplyVolume()
    {
        if (bgmSource != null)
        {
            bgmSource.volume = masterVolume * bgmVolume;
        }
    }
}

