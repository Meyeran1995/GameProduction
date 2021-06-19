using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainCharacterAnimationStageController : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MainCharacterMovement playerMovement;
    [Header("Animation Stages")]
    [SerializeField] private RuntimeAnimatorController[] stages;
    private int featherCount, currentStage;

    public const int STAGE_THRESHOLD = 5;

    private void Awake()
    {
        if(playerAnimator == null)
            playerAnimator = GetComponent<Animator>();
        if (playerMovement == null)
            playerMovement = GetComponent<MainCharacterMovement>();
    }

    private void Start() => RegisterWithHandler();

    [UsedImplicitly]
    public void OnFeatherCollected()
    {
        if (++featherCount % STAGE_THRESHOLD != 0 || currentStage >= stages.Length - 1) return;

        StartCoroutine(TransitionToNewStage());
    }

    private IEnumerator TransitionToNewStage()
    {
        playerAnimator.SetTrigger("Transition");

        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("End"));

        playerAnimator.runtimeAnimatorController = stages[++currentStage];
        playerAnimator.SetBool("MaxSpeedReached", playerMovement.MaxSpeedReached);
        playerAnimator.SetBool("CanMove", playerMovement.CanMove);
    }

    [UsedImplicitly]
    public void OnMaxSpeedReached() => playerAnimator.SetBool("MaxSpeedReached", true);

    [UsedImplicitly]
    public void OnMaxSpeedLost() => playerAnimator.SetBool("MaxSpeedReached", false);

    public void OnEndOfGameReached() => playerAnimator.SetTrigger("EndingReached");

    public void Restart()
    {
        featherCount = 0;
        currentStage = 0;
        playerAnimator.SetBool("MaxSpeedReached", false);
        playerAnimator.runtimeAnimatorController = stages[currentStage];
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
