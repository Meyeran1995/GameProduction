using UnityEngine;

/// <summary>
/// State when the character is hit and waiting for obstacles to be removed
/// </summary>
public class CrouchedState : AState
{
    private readonly MainCharacterCollisionEvaluator collisionEval;
    private readonly MainCharacterMovement movement;
    private readonly Animator animator;

    public CrouchedState(MainCharacterCollisionEvaluator collisionEval, Animator animator, float exitTime = 0f) : base(exitTime)
    {
        this.collisionEval = collisionEval;
        this.animator = animator;
        movement = collisionEval.GetComponent<MainCharacterMovement>();
    }

    public CrouchedState(MainCharacterCollisionEvaluator collisionEval, MainCharacterMovement movement, Animator animator, float exitTime = 0f) : base(exitTime)
    {
        this.collisionEval = collisionEval;
        this.animator = animator;
        this.movement = movement;
    }

    public override void OnStateEnter()
    {
        animator.SetTrigger("Staggered");
        animator.SetBool("CanMove", false);
        movement.StaggerCharacterMovement();
    }

    public override void OnStateExit(AState newState)
    {
        movement.RestartSpeedGain();
        animator.SetBool("CanMove", true);
        movement.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnFixedUpdate(float delta)
    {
        if(!collisionEval.QueryForFrontalCollisions())
            PlayerStateMachine.Instance.ChangeState(new MovingState());
    }
}
