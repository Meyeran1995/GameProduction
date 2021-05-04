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

    private Coroutine transferRoutine;

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

        //InputControls.Player.Protect.performed += OnStaminaTransferPerformed;
    }

    /// <summary>
    /// Begins transferring stamina from side- to main character
    /// </summary>
    /// <param name="context"></param>
    private void OnStaminaTransferBegin(InputAction.CallbackContext context)
    {
        movement.RegainStamina();
        bubble.StartResourceDepletion();
        transferRoutine = StartCoroutine(ObserveEnergyWhileTransferring(context));
    }

    /// <summary>
    /// Ends transferring stamina from side- to main character
    /// </summary>
    /// <param name="context"></param>
    private void OnStaminaTransferEnd(InputAction.CallbackContext context)
    {
        movement.StopStaminaRegain();
        bubble.EndResourceDepletion();

        InputControls.Player.Protect.started -= OnStaminaTransferBegin;
        InputControls.Player.Protect.started += OnBeginBubbleExpansion;

        InputControls.Player.Protect.canceled -= OnStaminaTransferEnd;
        InputControls.Player.Protect.canceled += OnEndBubbleExpansion;

        if(transferRoutine == null) return;

        StopCoroutine(transferRoutine);
    }

    /// <summary>
    /// Stop stamina regeneration process when either stamina bar reaches an end
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private IEnumerator ObserveEnergyWhileTransferring(InputAction.CallbackContext context)
    {
        yield return new WaitWhile(() => bubble.HasEnergyLeft && !movement.HasMaximumStamina);

        OnStaminaTransferEnd(context);
    }

    #endregion
}
