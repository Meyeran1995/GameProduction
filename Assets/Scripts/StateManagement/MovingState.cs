using UnityEngine;

/// <summary>
/// Default state in which the character is moving forward
/// </summary>
public class MovingState : AState
{
    public MovingState(MainCharacterMovement movement) : base(movement)
    {
    }

    public override void OnStateEnter()
    {
        movement.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnStateExit(AState newState)
    {
    }

    public override void OnUpdate(float delta)
    {
    }
}
