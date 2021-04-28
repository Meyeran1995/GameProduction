using UnityEngine;

public class SlowedState : AState
{
    public SlowedState(GameObject owner) : base(owner)
    {
    }

    public override void OnStateEnter() => movement.SlowDownCharacter();
    public override void OnStateExit(AState newState) => movement.RegainSpeed();

    public override void OnUpdate(float delta)
    {
    }
}
