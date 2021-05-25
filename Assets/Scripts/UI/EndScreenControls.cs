using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenControls : AListenerEnabler
{
    public static bool IsQuitting { get; private set; }

    [UsedImplicitly]
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    [UsedImplicitly]
    public void QuitGame()
    {
        IsQuitting = true;
        Application.Quit();
    }
}
