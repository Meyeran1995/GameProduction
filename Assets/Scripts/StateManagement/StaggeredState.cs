using UnityEngine;

/// <summary>
/// State after being hit by an obstacle
/// </summary>
public class StaggeredState : AState
{
    public StaggeredState(PlayerStateMachine owner, float exitTime = 0) : base(owner, exitTime)
    {
    }

    public override void OnStateEnter()
    {
        owner.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        owner.Movement.StaggerCharacterMovement();
    }

    public override void OnStateExit(AState newState)
    {
        if (newState is DownedState)
            return;

        owner.Movement.RestartSpeedGain();
    }

    public override void OnUpdate(float delta)
    {
    }
}
