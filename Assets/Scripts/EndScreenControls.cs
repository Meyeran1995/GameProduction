using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenControls : AListenerEnabler
{
    [UsedImplicitly]
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    [UsedImplicitly]
    public void QuitGame()
    {
        Application.Quit();
    }
}
