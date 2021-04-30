using UnityEngine;

/// <summary>
/// State when character stamina is empty and the character is thus downed
/// </summary>
public class DownedState : AState
{
    public DownedState(MainCharacterMovement movement) : base(movement)
    {
    }

    public override void OnStateEnter() => movement.GetComponent<SpriteRenderer>().color = Color.red;

    public override void OnStateExit(AState newState)
    {
        movement.RegainSpeed();
        movement.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void OnUpdate(float delta)
    {
    }
}
