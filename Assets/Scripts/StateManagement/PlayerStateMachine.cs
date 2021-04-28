using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private AState currentState;

    public static PlayerStateMachine Instance;

    private int numberOfCollidingObjects;
    //private MainCharacterMovement movement;

    private void Awake()
    {
        Instance = this;
        currentState = new MovingState(gameObject);
        //movement = GetComponent<MainCharacterMovement>();
    }

    public void ChangeState(AState newState)
    {
        currentState.OnStateExit(newState);
        currentState = newState;
        currentState.OnStateEnter();
    }

    private void Update()
    {
        currentState.OnUpdate(Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<BubbleExpander>())
        //{
        //    ChangeState(new MovingState(gameObject));
        //    return;
        //}
        
        if(numberOfCollidingObjects == 0)
        {
            ChangeState(new StaggeredState(gameObject, 2f));
        }

        numberOfCollidingObjects++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (--numberOfCollidingObjects == 0)
        {
            ChangeState(new MovingState(gameObject));
        }
    }
}
