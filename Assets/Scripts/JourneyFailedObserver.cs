using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class JourneyFailedObserver : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private AutoFillResourceBar companionBar;
    [SerializeField] private GameObject continueButton, menuButton;
    private Coroutine waitRoutine;
    private bool gameLost;

    private void Awake() => RegisterWithHandler();

    private void Start() => waitRoutine = StartCoroutine(ObserveRemainingEnergy());

    private IEnumerator ObserveRemainingEnergy()
    {
        yield return new WaitUntil(() => companionBar.IsDepleted);

        if(PlayerStateMachine.Instance.CurrentState is MovingState) yield break;
        
        transform.GetChild(0).gameObject.SetActive(true);
        continueButton.SetActive(false);
        menuButton.SetActive(false);
        gameLost = true;
    }

    [UsedImplicitly] 
    public void OnSpeedLost()
    {
        if(waitRoutine != null)
            StopCoroutine(waitRoutine);
        waitRoutine = StartCoroutine(ObserveRemainingEnergy());
    }

    [UsedImplicitly]
    public void OnMovementRestarted()
    {
        if (waitRoutine != null)
            StopCoroutine(waitRoutine);
        waitRoutine = null;
    }

    public void Restart()
    {
        if (!gameLost) return;

        continueButton.SetActive(true);
        menuButton.SetActive(true);
        gameLost = false;
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
