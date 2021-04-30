using UnityEngine;

/// <summary>
/// State when the characters movement speed is reduced e.g. through wind
/// </summary>
public class SlowedState : AState
{
    public SlowedState(MainCharacterMovement movement) : base(movement)
    {
    }

    public override void OnStateEnter()
    {
        movement.GetComponent<SpriteRenderer>().color = Color.blue;
        movement.SlowDownCharacter();
        Debug.Log("Slowed");
    }

    public override void OnStateExit(AState newState) => movement.RegainSpeed();

    public override void OnUpdate(float delta)
    {
    }
}
