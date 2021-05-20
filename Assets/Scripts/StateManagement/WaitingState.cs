using UnityEngine;

/// <summary>
/// State when the character is hit and waiting for obstacles to be removed
/// </summary>
public class WaitingState : AState
{
    private readonly MainCharacterCollisionEvaluator collisionEval;
    private readonly MainCharacterMovement movement;

    public WaitingState(GameObject owner, MainCharacterCollisionEvaluator collisionEval, float exitTime = 0f) : base(owner, exitTime)
    {
        this.collisionEval = collisionEval;
        movement = owner.GetComponent<MainCharacterMovement>();
    }

    public override void OnStateEnter()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.red;
        movement.StaggerCharacterMovement();
    }

    public override void OnStateExit(AState newState)
    {
        movement.RestartSpeedGain();
        owner.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnFixedUpdate(float delta)
    {
        if(!collisionEval.QueryForFrontalCollisions())
            PlayerStateMachine.Instance.ChangeState(new MovingState(owner));
    }
}
