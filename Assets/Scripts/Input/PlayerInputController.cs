using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : AListenerEnabler
{
    public GameInputsDefault InputControls { get; private set; }
    public static bool IsQuitting;

    [SerializeField] private MainCharacterMovement movement;
    [SerializeField] private BubbleExpander bubble;

    /// <summary>
    /// Is the character already getting back up? Then this routine will not be null
    /// </summary>
    private Coroutine getUpRoutine;

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

    #region Controls while knocked down

    [UsedImplicitly]
    public void OnMainCharacterStaminaDepleted()
    {
        bubble.StopExpanding();

        InputControls.Player.Protect.started += OnStaminaTransferBegin;
        InputControls.Player.Protect.started -= OnBeginBubbleExpansion;

        InputControls.Player.Protect.canceled += OnStaminaTransferEnd;
        InputControls.Player.Protect.canceled -= OnEndBubbleExpansion;
    }

    /// <summary>
    /// Begins transferring stamina from side- to main character
    /// </summary>
    /// <param name="context"></param>
    private void OnStaminaTransferBegin(InputAction.CallbackContext context)
    {
        if (getUpRoutine != null) return;

        movement.RegainStamina();
        bubble.StartResourceDepletion();
    }

    /// <summary>
    /// Ends transferring stamina from side- to main character
    /// </summary>
    /// <param name="context"></param>
    private void OnStaminaTransferEnd(InputAction.CallbackContext context)
    {
        if(getUpRoutine != null) return;

        getUpRoutine = StartCoroutine(OnGetUp());
    }

    /// <summary>
    /// Routine to change back controls after stamina transfer has ended
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnGetUp()
    {
        yield return new WaitForSeconds(movement.TimeToGetUp);

        movement.StopStaminaRegain();
        bubble.EndResourceDepletion();

        InputControls.Player.Protect.started -= OnStaminaTransferBegin;
        InputControls.Player.Protect.started += OnBeginBubbleExpansion;

        InputControls.Player.Protect.canceled -= OnStaminaTransferEnd;
        InputControls.Player.Protect.canceled += OnEndBubbleExpansion;
    }

    #endregion
}
