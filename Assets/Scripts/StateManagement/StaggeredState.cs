using UnityEngine;

public class StaggeredState : AState
{
    private float exitTime;

    public StaggeredState(GameObject owner, float exitTime) : base(owner)
    {
        this.exitTime = exitTime;
    }

    public override void OnStateEnter() => movement.StaggerCharacterMovement();

    public override void OnStateExit(AState newState)
    {
        if (newState is DownedState)
            return;

        movement.RegainSpeed();
    }

    public override void OnUpdate(float delta)
    {
        //exitTime -= delta;

        //if (exitTime <= 0f)
        //{
        //    PlayerStateMachine.Instance.ChangeState(new DownedState(movement.gameObject));
        //}
    }
}
