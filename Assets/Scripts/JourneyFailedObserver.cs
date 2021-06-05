using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class JourneyFailedObserver : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private AutoFillResourceBar companionBar;
    [SerializeField] private GameObject logo, youLostText;

    [Header("Buttons")] 
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject tryAgainButton, continueButton, menuButton;
    private Coroutine waitRoutine;
    private bool gameLost;

    [Header("Waiting")] 
    [SerializeField]
    [Tooltip("How much time will pass between failing and displaying of the game over screen?")]
    [Range(1f, 3f)]
    private float transitionTime;

    private void Start() => RegisterWithHandler();

    private IEnumerator ObserveRemainingEnergy()
    {
        yield return new WaitUntil(() => PlayerStateMachine.Instance.CurrentState is CrouchedState);

        menuButton.SetActive(false);

        yield return new WaitForSeconds(transitionTime);

        transform.GetChild(0).gameObject.SetActive(true);

        continueButton.SetActive(false);
        logo.SetActive(false);
        restartButton.SetActive(false);

        gameLost = true;
        youLostText.SetActive(true);
        tryAgainButton.SetActive(true);
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
        logo.SetActive(true);
        restartButton.SetActive(true);
        menuButton.SetActive(true);

        gameLost = false;
        youLostText.SetActive(false);
        tryAgainButton.SetActive(false);
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
