using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : AMultiListenerEnabler
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
        InputControls.Player.PauseGame.started += GameObject.FindGameObjectWithTag("PauseControls").GetComponent<PauseModeControls>().TogglePause;
    }

    private void Start() => DisableControls();

    protected override void OnEnable()
    {
        EnableControls();
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        DisableControls();
        base.OnDisable();
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    private void OnBeginBubbleExpansion(InputAction.CallbackContext context) => bubble.StartExpanding();

    private void OnEndBubbleExpansion(InputAction.CallbackContext context) => bubble.StopExpanding();

    [UsedImplicitly]
    public void StopListeningForInput()
    {
        InputControls.Player.Protect.Disable();
        InputControls.Player.Aim.Disable();
    }

    [UsedImplicitly]
    public void ContinueListeningForInput()
    {
        InputControls.Player.Protect.Enable();
        InputControls.Player.Aim.Enable();
    }

    public void EnableControls()
    {
        InputControls.Player.Protect.Enable();
        InputControls.Player.Aim.Enable();
        InputControls.Player.PauseGame.Enable();
    }

    public void DisableControls()
    {
        InputControls.Player.Protect.Disable();
        InputControls.Player.Aim.Disable();
        InputControls.Player.PauseGame.Disable();
    }
}
