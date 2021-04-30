using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public AState CurrentState { get; private set; }

    public static PlayerStateMachine Instance;

    public int NumberOfCollidingObjects { get; private set; }
    private MainCharacterMovement movement;

    private void Awake()
    {
        Instance = this;
        CurrentState = new MovingState(gameObject);
        movement = GetComponent<MainCharacterMovement>();
    }

    public void ChangeState(AState newState)
    {
        if (newState.GetType() == CurrentState.GetType()) return;
        if (CurrentState.GetType() == typeof(DownedState) && newState.GetType() != typeof(MovingState)) return;

        CurrentState.OnStateExit(newState);
        CurrentState = newState;
        CurrentState.OnStateEnter();
    }

    private void Update()
    {
        CurrentState.OnUpdate(Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<BubbleExpander>())
        //{
        //    ChangeState(new MovingState(gameObject));
        //    return;
        //}
        
        if(NumberOfCollidingObjects == 0)
        {
            ChangeState(new StaggeredState(gameObject, 2f));
        }

        NumberOfCollidingObjects++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (--NumberOfCollidingObjects == 0 && movement.HasStamina)
        {
            ChangeState(new MovingState(gameObject));
        }
    }
}
