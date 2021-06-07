using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainCharacterAnimationStageController : AListenerEnabler, IRestartable
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MainCharacterMovement playerMovement;
    [SerializeField] private RuntimeAnimatorController[] stages;
    [SerializeField] private int threshHoldValue;
    private int featherCount, currentStage;

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
        if (++featherCount % threshHoldValue != 0 || currentStage >= stages.Length - 1) return;

        playerAnimator.runtimeAnimatorController = stages[++currentStage];
        playerAnimator.SetBool("CanMove", playerMovement.CanMove);
    }

    public void Restart()
    {
        featherCount = 0;
        currentStage = 0;
        playerAnimator.runtimeAnimatorController = stages[currentStage];
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
