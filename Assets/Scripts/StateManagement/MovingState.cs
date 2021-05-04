using UnityEngine;

/// <summary>
/// Default state in which the character is moving forward
/// </summary>
public class MovingState : AState
{
    public MovingState(PlayerStateMachine owner, float exitTime = 0) : base(owner, exitTime)
    {
    }

    public override void OnStateEnter()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnStateExit(AState newState)
    {
    }

    public override void OnUpdate(float delta)
    {
    }
}
