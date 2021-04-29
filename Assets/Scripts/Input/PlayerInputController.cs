using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public GameInputsDefault InputControls => inputControls;
    private GameInputsDefault inputControls;
    public static bool IsQuitting;
    [SerializeField] private BubbleExpander bubble;

    private void Awake()
    {
        inputControls = new GameInputsDefault();
        inputControls.Player.Protect.started += OnBeginBubbleExpansion;
        inputControls.Player.Protect.canceled += OnEndBubbleExpansion;
    }

    public void OnEnable()
    {
        inputControls.Player.Protect.Enable();
        inputControls.Player.Aim.Enable();
    }

    private void OnDisable()
    {
        inputControls.Player.Protect.Disable();
        inputControls.Player.Aim.Disable();
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    private void OnBeginBubbleExpansion(InputAction.CallbackContext context)
    {
        if (PlayerStateMachine.Instance.CurrentState is DownedState)
        {
            GetComponent<MainCharacterMovement>().RegainStamina();
        }

        bubble.IsExpanding = true;
    }

    private void OnEndBubbleExpansion(InputAction.CallbackContext context)
    {
        if (PlayerStateMachine.Instance.CurrentState is DownedState)
        {
            GetComponent<MainCharacterMovement>().StopStaminaRegain();
        }

        bubble.IsExpanding = false;
    }
}
