using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private GameInputsDefault inputControls;
    public static bool IsQuitting;
    private Camera mainCam;
    [SerializeField] private BubbleExpander bubble;

    private void Awake()
    {
        mainCam = Camera.main;

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
    }

    private void OnDisable()
    {
        inputControls.Player.Protect.Disable();
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }
}
