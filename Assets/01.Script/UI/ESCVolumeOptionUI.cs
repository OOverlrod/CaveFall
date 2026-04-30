using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ESCVolumeOptionUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject optionPanel;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Value Text")]
    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private TextMeshProUGUI bgmValueText;
    [SerializeField] private TextMeshProUGUI sfxValueText;

    [Header("Button Sound")]
    [SerializeField] private AudioClip buttonSfx;
    [SerializeField] private float buttonSfxVolume = 0.5f;

    private void Start()
    {
        if (SoundManager.Instance == null) return;

        SetupSlider(masterSlider, SoundManager.Instance.MasterVolume, SoundManager.Instance.SetMasterVolume, masterValueText);
        SetupSlider(bgmSlider, SoundManager.Instance.BgmVolume, SoundManager.Instance.SetBgmVolume, bgmValueText);
        SetupSlider(sfxSlider, SoundManager.Instance.SfxVolume, SoundManager.Instance.SetSfxVolume, sfxValueText);

        if (optionPanel != null)
            optionPanel.SetActive(false);
    }

    private void Update()
    {
        if (optionPanel == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf)
                CloseOption();
            else
                OpenOption();
        }
    }

    private void SetupSlider(Slider slider, float soundValue, UnityEngine.Events.UnityAction<float> setVolume, TextMeshProUGUI valueText)
    {
        if (slider == null) return;

        slider.minValue = 0;
        slider.maxValue = 10;
        slider.wholeNumbers = true;

        slider.value = soundValue * 10f;

        slider.onValueChanged.AddListener(value =>
        {
            setVolume(value / 10f);
            UpdateVolumeText(slider, valueText);
        });

        UpdateVolumeText(slider, valueText);
    }

    private void UpdateVolumeText(Slider slider, TextMeshProUGUI text)
    {
        if (slider == null || text == null) return;

        int level = Mathf.RoundToInt(slider.value);
        text.text = level.ToString();
    }

    public void OpenOption()
    {
        PlayButtonSfx();

        if (optionPanel != null)
            optionPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseOption()
    {
        PlayButtonSfx();

        if (optionPanel != null)
            optionPanel.SetActive(false);

        Time.timeScale = 1f;
    }
    private void PlayButtonSfx()
    {
        if (SoundManager.Instance != null && buttonSfx != null)
        {
            SoundManager.Instance.PlaySfxOneShot(buttonSfx, buttonSfxVolume);
        }
    }
}