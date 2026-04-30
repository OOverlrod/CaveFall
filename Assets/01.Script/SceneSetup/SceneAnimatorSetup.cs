using UnityEngine;

// 각 씬 애니메이터 설정
public class SceneAnimatorSetup : MonoBehaviour
{
    public enum Mode
    {
        Side,
        Vertical
    }

    [SerializeField] private Mode mode;

    private void Start()
    {
        PlayerAnimSwitcher switcher = FindFirstObjectByType<PlayerAnimSwitcher>();

        if (switcher == null) return;

        if (mode == Mode.Side)
            switcher.SetSideMode();
        else
            switcher.SetVerticalMode();
    }
}