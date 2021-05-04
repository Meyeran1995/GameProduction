using UnityEngine;

/// <summary>
/// State when character stamina is empty and the character is thus downed
/// </summary>
public class DownedState : AState
{
    public DownedState(PlayerStateMachine owner, float exitTime = 0) : base(owner, exitTime)
    {
    }

    public override void OnStateEnter() => owner.GetComponent<SpriteRenderer>().color = Color.red;

    public override void OnStateExit(AState newState)
    {
        owner.Movement.RestartSpeedGain();
        owner.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnUpdate(float delta)
    {
    }
}
