using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : AListenerEnabler
{
    public GameInputsDefault InputControls { get; private set; }
    public static bool IsQuitting;

    [SerializeField] private MainCharacterMovement movement;
    [SerializeField] private BubbleExpander bubble;

    private void Awake()
    {
        InputControls = new GameInputsDefault();
        InputControls.Player.Protect.started += OnBeginBubbleExpansion;
        InputControls.Player.Protect.canceled += OnEndBubbleExpansion;
    }

    protected override void OnEnable()
    {
        InputControls.Player.Protect.Enable();
        InputControls.Player.Aim.Enable();
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        InputControls.Player.Protect.Disable();
        InputControls.Player.Aim.Disable();
        base.OnDisable();
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    #region Controls while main character is moving

    private void OnBeginBubbleExpansion(InputAction.CallbackContext context) => bubble.StartExpanding();

    private void OnEndBubbleExpansion(InputAction.CallbackContext context) => bubble.StopExpanding();

    #endregion
}
