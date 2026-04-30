using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleVolumeOptionUI : MonoBehaviour
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

    private void Start()
    {
        if (SoundManager.Instance == null) return;

        if (masterSlider != null)
        {
            masterSlider.value = SoundManager.Instance.MasterVolume * 10f;
            masterSlider.onValueChanged.AddListener(SoundManager.Instance.SetMasterVolume);
            masterSlider.onValueChanged.AddListener(_ => UpdateVolumeText(masterSlider, masterValueText));
            UpdateVolumeText(masterSlider, masterValueText);
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = SoundManager.Instance.BgmVolume * 10f;
            bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBgmVolume);
            bgmSlider.onValueChanged.AddListener(_ => UpdateVolumeText(bgmSlider, bgmValueText));
            UpdateVolumeText(bgmSlider, bgmValueText);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = SoundManager.Instance.SfxVolume * 10f;
            sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSfxVolume);
            sfxSlider.onValueChanged.AddListener(_ => UpdateVolumeText(sfxSlider, sfxValueText));
            UpdateVolumeText(sfxSlider, sfxValueText);
        }

        if (optionPanel != null)
            optionPanel.SetActive(false);
    }
    private void Update()
    {
        if (optionPanel == null) return;

        // 옵션창이 열려 있을 때만 ESC 입력 받기
        if (optionPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOption();
        }
    }

    private void UpdateVolumeText(Slider slider, TextMeshProUGUI text)
    {
        if (slider == null || text == null) return;

        int percent = Mathf.RoundToInt(slider.value * 1f);
        text.text = percent + "";
    }

    public void OpenOption()
    {
        if (optionPanel != null)
            optionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        if (optionPanel != null)
            optionPanel.SetActive(false);
    }
}
