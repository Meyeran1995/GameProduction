using UnityEngine;

/// <summary>
/// A trap that slows the character which needs to be blocked by the side character
/// </summary>
public class WindTrap : SnowTrap
{
    [SerializeField] private Transform origin;
    private Transform bubble;

    protected override void TriggerTrap(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsInsideTrap = true;

            if (bubble == null || !IsBubbleInFrontOfCharacter())
            {
                PlayerStateMachine.Instance.ChangeState(new SlowedState(MainCharacter));
            }
        }
        else
        {
            bubble = collision.transform;
        }
    }

    private void FixedUpdate()
    {
        if (!playerIsInsideTrap || bubble == null) return;

        if (IsBubbleInFrontOfCharacter())
        {
            PlayerStateMachine.Instance.ChangeState(new MovingState(MainCharacter));
        }
        else
        {
            PlayerStateMachine.Instance.ChangeState(new SlowedState(MainCharacter));
        }
    }

    private bool IsBubbleInFrontOfCharacter() => Vector3.Distance(bubble.position, origin.position) <=
                                                 Vector3.Distance(MainCharacter.transform.position, origin.position);

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.ChangeState(new MovingState(MainCharacter));
            playerIsInsideTrap = false;
            gameObject.SetActive(false);
        }
        else if(playerIsInsideTrap)
        {
            bubble = null;
            PlayerStateMachine.Instance.ChangeState(new SlowedState(MainCharacter));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(origin.position, transform.position);
        Gizmos.DrawWireCube(origin.position, origin.localScale / 2f);
        Gizmos.color = Color.red;
        // transform gizmo using this scripts transform matrix
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
