using UnityEngine;
using TMPro;

public class StageUI : MonoBehaviour
{
    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI stageText;

    private void Start()
    {
        if (StageProgressManager.Instance == null) return;

        // 초기 표시
        UpdateStage(StageProgressManager.Instance.CurrentLoopLevel);

        // 이벤트 등록
        StageProgressManager.Instance.OnStageChanged += UpdateStage;
    }

    private void OnDestroy()
    {
        if (StageProgressManager.Instance != null)
        {
            StageProgressManager.Instance.OnStageChanged -= UpdateStage;
        }
    }

    private void UpdateStage(int stage)
    {
        stageText.text = $"Stage\n\n{stage}";
    }
}
