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
        inputControls.Player.Protect.started += (InputAction.CallbackContext context) =>
        {
            bubble.IsExpanding = true;
        };
        inputControls.Player.Protect.canceled += (InputAction.CallbackContext context) =>
        {
            bubble.IsExpanding = false;
        };
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
}
