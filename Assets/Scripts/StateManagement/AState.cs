using UnityEngine;

public abstract class AState
{
    //protected readonly GameObject owningGameObject;
    protected readonly MainCharacterMovement movement;

    protected AState(GameObject owner)
    {
        movement = owner.GetComponent<MainCharacterMovement>();
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
