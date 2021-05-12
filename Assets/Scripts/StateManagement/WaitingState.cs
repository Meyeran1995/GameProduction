using UnityEngine;

/// <summary>
/// State when the character is hit and waiting for obstacles to be removed
/// </summary>
public class WaitingState : AState
{
    private readonly MainCharacterCollisionEvaluator collisionEval;

    public WaitingState(PlayerStateMachine owner, MainCharacterCollisionEvaluator collisionEval, float exitTime = 0f) : base(owner, exitTime)
    {
        this.collisionEval = collisionEval;
    }

    public override void OnStateEnter()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.red;
        owner.Movement.StaggerCharacterMovement();
    }

    public override void OnStateExit(AState newState)
    {
        owner.Movement.RestartSpeedGain();
        owner.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnFixedUpdate(float delta)
    {
        if(!collisionEval.QueryForFrontalCollisions())
            owner.ChangeState(new MovingState(owner));
    }
}
