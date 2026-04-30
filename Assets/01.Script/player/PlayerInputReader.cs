using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction jumpAction;
    private InputAction dashAction;

    public Vector2 MoveVector { get; private set; }
    public bool AttackPressedThisFrame { get; private set; }
    public bool JumpPressedThisFrame { get; private set; }
    public bool DashPressedThisFrame { get; private set; }
    public bool isMoving { get; private set; }

    [Header("Action Names")]
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string attackActionName = "Attack";
    [SerializeField] private string jumpActionName = "Jump";
    [SerializeField] private string dashActionName = "Sprint";

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput.currentActionMap == null || playerInput.currentActionMap.name != "Player")
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        ResolveActions();
    }

    private void Update()
    {
        MoveVector = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
        AttackPressedThisFrame = attackAction != null && attackAction.WasPerformedThisFrame();
        JumpPressedThisFrame = jumpAction != null && jumpAction.WasPerformedThisFrame();
        DashPressedThisFrame = dashAction != null && dashAction.WasPerformedThisFrame();

        isMoving = MoveVector.sqrMagnitude > 0.001f;
    }

    private void ResolveActions()
    {
        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogWarning("[PlayerInputReader] Action Asset 확인 필요");
            return;
        }

        moveAction = FindAction(moveActionName);
        attackAction = FindAction(attackActionName);
        jumpAction = FindAction(jumpActionName);
        dashAction = FindAction(dashActionName);
    }

    private InputAction FindAction(string actionName)
    {
        if (string.IsNullOrWhiteSpace(actionName))
        {
            return null;
        }

        InputAction action = playerInput.actions.FindAction(actionName, false);

        return action;
    }
}