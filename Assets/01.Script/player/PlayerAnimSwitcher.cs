using UnityEngine;

public class PlayerAnimSwitcher : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator;

    [Header("Animator Controllers")]
    [SerializeField] private RuntimeAnimatorController sideController;
    [SerializeField] private RuntimeAnimatorController verticalController;

    private bool isVerticalMode;

    private void Awake()
    {
        if (targetAnimator == null)
            targetAnimator = GetComponent<Animator>();
    }

    public void SetSideMode()
    {
        if (targetAnimator == null || sideController == null) return;
        if (!isVerticalMode && targetAnimator.runtimeAnimatorController == sideController) return;

        targetAnimator.runtimeAnimatorController = sideController;
        isVerticalMode = false;
    }

    public void SetVerticalMode()
    {
        if (targetAnimator == null || verticalController == null) return;
        if (isVerticalMode && targetAnimator.runtimeAnimatorController == verticalController) return;

        targetAnimator.runtimeAnimatorController = verticalController;
        isVerticalMode = true;
    }

}
