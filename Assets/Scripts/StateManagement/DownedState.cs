using UnityEngine;

public class DownedState : AState
{
    public DownedState(GameObject owner) : base(owner)
    {
    }

    public override void OnStateEnter()
    {
        movement.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override void OnStateExit(AState newState)
    {
        movement.RegainSpeed();
    }

    public override void OnUpdate(float delta)
    {
    }
}
