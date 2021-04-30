using UnityEngine;

public class SnowTrap : ATrap
{
    protected bool playerIsInsideTrap;
    protected static MainCharacterMovement MainCharacter;

    protected virtual void Awake()
    {
        if (MainCharacter == null)
            MainCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterMovement>();
    }

    protected override void TriggerTrap(Collider2D collision)
    {
        if(playerIsInsideTrap)
        {
            PlayerStateMachine.Instance.ChangeState(new MovingState(MainCharacter));
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.ChangeState(new SlowedState(MainCharacter));
            playerIsInsideTrap = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.ChangeState(new MovingState(MainCharacter));
            playerIsInsideTrap = false;
        }
        else if (playerIsInsideTrap)
        {
            PlayerStateMachine.Instance.ChangeState(new SlowedState(MainCharacter));
        }
    }
}
