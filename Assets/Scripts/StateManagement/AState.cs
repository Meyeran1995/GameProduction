/// <summary>
/// Abstract base class for a state with enter, exit events and update loop
/// </summary>
public abstract class AState
{
    protected readonly MainCharacterMovement movement;

    protected AState(MainCharacterMovement movement)
    {
        this.movement = movement;
    }

    /// <summary>
    /// Method to be called when entering the state
    /// </summary>
    public abstract void OnStateEnter();

    /// <summary>
    /// States update loop
    /// </summary>
    /// <param name="delta">Delta time</param>
    public abstract void OnUpdate(float delta);

    /// <summary>
    /// Method to be called upon exiting the state
    /// </summary>
    /// <param name="newState">State to transition to</param>
    public abstract void OnStateExit(AState newState);
}
