using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class TutorialProgressController : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private MusicPiece mainMenuMusicPiece;

    [Header("Intro")]
    [SerializeField] private AssetReference[] introScenes;
    private int currentScene;
    private Animator[] animators;

    [Header("Loading")]
    [SerializeField] private float[] loadProgress;
    [SerializeField] private Image loadProgressImage;
    private int currentSceneLoadIndex;
    private AsyncOperationHandle<GameObject> currentHandle;
    private Coroutine loadRoutine;

    private void Awake()
    {
        currentScene = 0;
        animators = new Animator[introScenes.Length];

        loadProgress = new float[introScenes.Length];
        for (int i  = 0;  i < loadProgress.Length; i++)
        {
            loadProgress[i] = 0f;
        }

        loadRoutine = StartCoroutine(LoadClips());
        loadProgressImage.fillAmount = 0f;
    }

    private IEnumerator LoadClips()
    {
        for (int i = 0; i < introScenes.Length; i++)
        {
            currentHandle = introScenes[i].InstantiateAsync(transform);
            currentSceneLoadIndex = i;
            currentHandle.Completed += handle => handle.Result.gameObject.SetActive(false);

            yield return currentHandle;

            var currentAnimator = currentHandle.Result.GetComponent<Animator>();
            animators[i] = currentAnimator;

            loadProgress[currentSceneLoadIndex] = 1f / introScenes.Length;
            currentAnimator.transform.SetAsFirstSibling();
        }

        StartCoroutine(FadeOutLogo());
        loadRoutine = null;
    }

    private float GetLoadProgress()
    {
        float progress = 0f;

        foreach (var p in loadProgress)
        {
            progress += p;
        }

        return progress;
    }

    private void FixedUpdate()
    {
        if(loadRoutine == null) return;

        loadProgress[currentSceneLoadIndex] = currentHandle.PercentComplete / introScenes.Length;
        loadProgressImage.fillAmount = GetLoadProgress();
    }

    private IEnumerator PlayClip()
    {
        if (currentScene < introScenes.Length)
        {
            var currentAnimator = animators[currentScene];
            currentAnimator.gameObject.SetActive(true);
            currentAnimator.SetTrigger("SceneStart");

            yield return new WaitUntil(() => currentAnimator.GetCurrentAnimatorStateInfo(0).IsTag("End"));

            if (++currentScene < introScenes.Length)
            {
                currentAnimator.gameObject.SetActive(false);
            }

            StartCoroutine(PlayClip());
        }
        else
        {
            yield return new WaitUntil(() => animators[introScenes.Length - 1].GetCurrentAnimatorStateInfo(0).IsTag("End"));

            StartCoroutine(FadeToMain());
        }
    }

    [UsedImplicitly]
    public void SkipIntro()
    {
        StopAllCoroutines();
        loadRoutine = null;
        Addressables.ReleaseInstance(currentHandle);
        StartCoroutine(FadeToMain());
    }

    private IEnumerator FadeToMain()
    {
        mainMenuMusicPiece.PlaySolo();

        if (animators[0] != null)
        {
            var fade = currentScene < introScenes.Length ? animators[currentScene].GetComponent<ImageFadeEffect>() : animators[introScenes.Length - 1].GetComponent<ImageFadeEffect>();

            yield return StartCoroutine(fade.FadeOut());
        }

        StartCoroutine(CleanUpIntro());
    }

    private IEnumerator FadeOutLogo()
    {
        var fade = loadProgressImage.GetComponent<ImageFadeEffect>();

        yield return StartCoroutine(fade.FadeOut());

        StartCoroutine(PlayClip());
        Destroy(loadProgressImage);
    }

    private IEnumerator CleanUpIntro()
    {
        foreach (var t in animators)
        {
            if (t == null) continue;
            
            Addressables.ReleaseInstance(t.gameObject);
            yield return null;
        }

        Destroy(gameObject);
    }
}
