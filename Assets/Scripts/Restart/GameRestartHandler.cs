using System.Collections.Generic;
using JetBrains.Annotations;

public class GameRestartHandler : AListenerEnabler
{
    private static readonly List<IRestartable> objectsForRestart = new List<IRestartable>();

    public static void RegisterRestartable(IRestartable restartable) => objectsForRestart.Add(restartable);

    public static void RegisterRestartable(IRestartable restartable, int index) => objectsForRestart.Insert(index, restartable);

    public static void UnRegisterRestartable(IRestartable restartable) => objectsForRestart.Remove(restartable);

    [UsedImplicitly]
    public void RestartGame()
    {
        foreach (var restartable in objectsForRestart)
        {
            restartable.Restart();
        }
    }
}
