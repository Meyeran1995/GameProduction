/// <summary>
/// Interface for setting a GameObject back to its original state
/// </summary>
public interface IRestartable
{
    /// <summary>
    /// Apply previously logged data to reset to original state at the start of the game
    /// </summary>
    public void Restart();

    /// <summary>
    /// Registers itself with a restart handler
    /// </summary>
    public void RegisterWithHandler();
}
