using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(Animator))]
public class MainCharacterAnimationStageController : AMultiListenerEnabler, IRestartable
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MainCharacterMovement playerMovement;
    [Header("Animation Stages")]
    [SerializeField] private RuntimeAnimatorController[] stages;
    //[SerializeField] private AssetReferenceGameObject[] stageRefs;
    private int featherCount, currentStage;
    //private int currentLoadIndex;
    //private AsyncOperationHandle<GameObject> currentHandle;

    public const int STAGE_THRESHOLD = 5;

    private void Awake()
    {
        if(playerAnimator == null)
            playerAnimator = GetComponent<Animator>();
        if (playerMovement == null)
            playerMovement = GetComponent<MainCharacterMovement>();
        //StartCoroutine(LoadStages());
    }

    private void Start() => RegisterWithHandler();

    [UsedImplicitly]
    public void OnFeatherCollected()
    {
        if (++featherCount % STAGE_THRESHOLD != 0 || currentStage >= stages.Length - 1) return;

        playerAnimator.runtimeAnimatorController = stages[++currentStage];
        playerAnimator.SetBool("MaxSpeedReached", playerMovement.MaxSpeedReached);
        playerAnimator.SetBool("CanMove", playerMovement.CanMove);
    }

    [UsedImplicitly]
    public void OnMaxSpeedReached() => playerAnimator.SetBool("MaxSpeedReached", true);

    [UsedImplicitly]
    public void OnMaxSpeedLost() => playerAnimator.SetBool("MaxSpeedReached", false);

    //private IEnumerator LoadStages()
    //{
    //    LoadNextStage();

    //    yield return currentHandle;

    //    currentLoadIndex++;

    //    LoadNextStage();
    //}

    //private void LoadNextStage()
    //{
    //    currentHandle = stageRefs[currentLoadIndex].LoadAssetAsync();
    //    currentHandle.Completed += OnStageLoaded;
    //}

    //private void OnStageLoaded(AsyncOperationHandle<GameObject> handle)
    //{
    //    stages[currentLoadIndex + 1] = handle.Result.GetComponent<Animator>().runtimeAnimatorController;
    //    GameObject memoryHolder = Instantiate(handle.Result, transform);
    //    memoryHolder.SetActive(false);
    //    //if(++currentLoadIndex < stageRefs.Length)
    //    //    LoadNextStage();
    //}

    public void Restart()
    {
        featherCount = 0;
        currentStage = 0;
        playerAnimator.SetBool("MaxSpeedReached", false);
        playerAnimator.runtimeAnimatorController = stages[currentStage];
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
