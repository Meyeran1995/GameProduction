using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseModeControls : MonoBehaviour
{
    [SerializeField] private UnityEvent beginPause, endPause;
    private bool isPaused;

    /// <summary>
    /// Toggles the games pause state
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
        {
            endPause.Invoke();
        }
        else
        {
            beginPause.Invoke();
        }

        isPaused = !isPaused;
    }

    /// <summary>
    /// Specific pause function for usage with the input system
    /// </summary>
    /// <param name="context"></param>
    public void TogglePause(InputAction.CallbackContext context) => TogglePause();
}
