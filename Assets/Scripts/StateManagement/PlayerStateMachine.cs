using System.Collections;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public AState CurrentState { get; private set; }

    public static PlayerStateMachine Instance { get; private set; }
    public MainCharacterMovement Movement { get; private set; }

    private Coroutine exitRoutine;

    [SerializeField] private GameEvent restartWalkingEvent;

    private void Awake()
    {
        Instance = this;
        Movement = GetComponent<MainCharacterMovement>();
        var collisionEvaluator = GetComponent<MainCharacterCollisionEvaluator>();
        CurrentState = new WaitingState(this, collisionEvaluator, collisionEvaluator.StaggerTime);
        CurrentState.OnStateEnter();
    }

    public void ChangeState(AState newState)
    {
        if (exitRoutine != null || newState.GetType() == CurrentState.GetType()) return;

        if (CurrentState.exitTime != 0f)
        {
            exitRoutine = StartCoroutine(WaitForExitTime(newState));
        }
        else
        {
            TransitionToNextState(newState);
        }

        if(newState is WaitingState) return;

        restartWalkingEvent.Raise();
    }

    private void TransitionToNextState(AState newState)
    {
        CurrentState.OnStateExit(newState);
        CurrentState = newState;
        CurrentState.OnStateEnter();
    }

    /// <summary>
    /// Waits for the current states exit time to elapse
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForExitTime(AState newState)
    {
        yield return new WaitForSeconds(CurrentState.exitTime);

        TransitionToNextState(newState);
        exitRoutine = null;
    }

    private void FixedUpdate()
    {
        CurrentState.OnFixedUpdate(Time.fixedDeltaTime);
    }
}
