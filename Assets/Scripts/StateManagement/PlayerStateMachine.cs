using System.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour, IRestartable
{
    public AState CurrentState { get; private set; }

    public static PlayerStateMachine Instance { get; private set; }

    private Coroutine exitRoutine;

    private void Awake()
    {
        Instance = this;
        var collisionEvaluator = GetComponent<MainCharacterCollisionEvaluator>();
        CurrentState = new WaitingState(gameObject, collisionEvaluator, collisionEvaluator.StaggerTime);
        CurrentState.OnStateEnter();
    }

    private void Start() => RegisterWithHandler();

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

    private void FixedUpdate() => CurrentState.OnFixedUpdate(Time.fixedDeltaTime);

    public void Restart()
    {
        if (exitRoutine != null)
            StopCoroutine(exitRoutine);
        exitRoutine = null;
        Awake();
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
