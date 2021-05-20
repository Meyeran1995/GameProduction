using UnityEngine;

/// <summary>
/// Abstract base class for a state with enter, exit events and update loop
/// </summary>
public abstract class AState
{
    protected readonly GameObject owner;
    public readonly float exitTime;

    protected AState(GameObject owner, float exitTime = 0f)
    {
        this.owner = owner;
        this.exitTime = exitTime;
    }

    /// <summary>
    /// Method to be called when entering the state
    /// </summary>
    public abstract void OnStateEnter();

    /// <summary>
    /// States update loop
    /// </summary>
    /// <param name="delta">Delta time</param>
    public abstract void OnFixedUpdate(float delta);

    /// <summary>
    /// Method to be called upon exiting the state
    /// </summary>
    /// <param name="newState">State to transition to</param>
    public abstract void OnStateExit(AState newState);
}
