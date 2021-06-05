using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class JourneyFailedObserver : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private AutoFillResourceBar companionBar;
    [SerializeField] private GameObject continueButton, menuButton;
    private Coroutine waitRoutine;
    private bool gameLost;

    private void Start() => RegisterWithHandler();

    private IEnumerator ObserveRemainingEnergy()
    {
        yield return new WaitUntil(() => PlayerStateMachine.Instance.CurrentState is CrouchedState);
        
        transform.GetChild(0).gameObject.SetActive(true);
        continueButton.SetActive(false);
        menuButton.SetActive(false);
        gameLost = true;
    }

    [UsedImplicitly] 
    public void OnEnergyDepleted()
    {
        CheckForActiveRoutine();
        waitRoutine = StartCoroutine(ObserveRemainingEnergy());
    }

    [UsedImplicitly]
    public void OnBeginEnergyRefill()
    {
        CheckForActiveRoutine();
        waitRoutine = null;
    }

    private void CheckForActiveRoutine()
    {
        if (waitRoutine == null) return;

        StopCoroutine(waitRoutine);
    }

    public void Restart()
    {
        CheckForActiveRoutine();
        waitRoutine = null;

        if (!gameLost) return;

        continueButton.SetActive(true);
        menuButton.SetActive(true);
        gameLost = false;
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
