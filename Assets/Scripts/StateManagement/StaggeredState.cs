using UnityEngine;

/// <summary>
/// State after being hit by an obstacle
/// </summary>
public class StaggeredState : AState
{
    public StaggeredState(MainCharacterMovement movement) : base(movement)
    {
    }

    public override void OnStateEnter()
    {
        movement.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        movement.StaggerCharacterMovement();
    }
    

    public override void OnStateExit(AState newState)
    {
        if (newState is DownedState)
            return;

        movement.RegainSpeed();
    }

    public override void OnUpdate(float delta)
    {
    }
}
