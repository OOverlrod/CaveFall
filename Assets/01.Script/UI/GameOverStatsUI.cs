using TMPro;
using UnityEngine;

public class GameOverStatsUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject menuPanel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI floorDestroyText;

    private void Start()
    {
        if (RunStatsManager.Instance == null) return;

        stageText.text = $"{RunStatsManager.Instance.ReachedStage}";
        killText.text = $"{RunStatsManager.Instance.MonsterKillCount}";
        floorDestroyText.text = $"{RunStatsManager.Instance.DestroyedFloorCount}";

        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    public void OnClickNext()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (menuPanel != null)
            menuPanel.SetActive(true);
    }
}