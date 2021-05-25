using System.Collections;
using JetBrains.Annotations;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TutorialProgressController : MonoBehaviour, IRestartable
{
    [SerializeField] private GameEvent startEvent;
    [SerializeField] private GameObject sideCharacter, mainMenu;
    private ImageFadeEffect mainMenuFade;

    [Header("Intro")]
    [SerializeField] private AssetReference[] introScenes;
    private int currentScene;
    private Animator currentAnimator, nextAnimator;

    private void Awake()
    {
        currentScene = 0;
        LoadNextClip();
    }

    private void Start()
    {
        mainMenuFade = mainMenu.GetComponent<ImageFadeEffect>();
        RegisterWithHandler();
    }

    [UsedImplicitly]
    public void SkipIntro()
    {
        StopAllCoroutines();
        StartCoroutine(FadeToGameplay());
    }

    [UsedImplicitly]
    public void StartIntro()
    {
        if(transform.childCount == 1)
            Awake();

        StartCoroutine(WaitForIntroLoad());
    }

    private IEnumerator WaitForIntroLoad()
    {
        yield return new WaitUntil(() => currentAnimator != null);
        yield return StartCoroutine(mainMenuFade.FadeOut());
        
        mainMenu.SetActive(false);
        currentAnimator.SetTrigger("SceneStart");
    }

    private AsyncOperationHandle<GameObject> LoadNextClip()
    {
        var handle = introScenes[currentScene].InstantiateAsync(transform);
        handle.Completed += OnClipLoaded;

        currentScene++;

        return handle;
    }

    private void OnClipLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (currentScene == 1)
        {
            currentAnimator = handle.Result.GetComponent<Animator>();
            currentAnimator.transform.SetAsFirstSibling();
            StartCoroutine(WatchForEndOfClip());
        }
        else
        {
            nextAnimator = handle.Result.GetComponent<Animator>();
            nextAnimator.transform.SetAsFirstSibling();
            nextAnimator.gameObject.SetActive(false);
        }
    }

    private IEnumerator WatchForEndOfClip()
    {
        if (currentScene < introScenes.Length)
        {
            yield return LoadNextClip();

            yield return new WaitUntil(() => currentAnimator.GetCurrentAnimatorStateInfo(0).IsTag("End"));

            nextAnimator.gameObject.SetActive(true);
            nextAnimator.SetTrigger("SceneStart");

            Addressables.ReleaseInstance(currentAnimator.gameObject);
            currentAnimator = nextAnimator;

            StartCoroutine(WatchForEndOfClip());
        }
        else
        {
            yield return new WaitUntil(() => currentAnimator.GetCurrentAnimatorStateInfo(0).IsTag("End"));

            StartCoroutine(FadeToGameplay());
        }
    }

    private IEnumerator FadeToGameplay()
    {
        var fade = currentAnimator.GetComponent<ImageFadeEffect>();

        yield return StartCoroutine(fade.FadeOut());

        StartGamePlay();
    }

    private void StartGamePlay()
    {
        foreach (Transform clip in transform)
        {
            if (clip.CompareTag("Skip"))
            {
                clip.gameObject.SetActive(false);
            }
            else
            {
                Addressables.ReleaseInstance(clip.gameObject);
            }    
        }

        startEvent.Raise();
        sideCharacter.SetActive(true);
        CollisionSoundController.UnmuteCollisions();
    }

    public void Restart()
    {
        if(!mainMenu.activeSelf) return;

        Awake();
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
