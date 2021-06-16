using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EndScreenControls : AListenerEnabler
{
    public static bool IsQuitting { get; private set; }

    [SerializeField] private StatTracker stats;

    [Header("Outro")] 
    [SerializeField] private AssetReferenceSprite[] goodEndingScenes;
    [SerializeField] private AssetReferenceSprite[] badEndingScenes;
    [SerializeField] private Image front, back;
    [SerializeField] private Button progressButton;
    private ImageFadeEffect fadeEffect;
    private int currentScene, activeSequenceLength;
    private bool isGoodEnding;

    [Header("Controls")]
    [SerializeField] private ImageFadeEffect[] controlsFadeEffects;

    [SerializeField] private GameObject controls;

    private void Awake()
    {
        fadeEffect = front.GetComponent<ImageFadeEffect>();
        progressButton.interactable = false;
    }

    private void OnPageLoaded(AsyncOperationHandle<Sprite> pageHandle)
    {
        if (pageHandle.Status != AsyncOperationStatus.Failed)
        {
            front.gameObject.SetActive(true);
            front.sprite = back.sprite;
            back.sprite = pageHandle.Result;

            fadeEffect.ResetEffect();
            StartCoroutine(FadeCurrentPage());
        }
        else
        {
            Debug.LogError($"Loading scene {currentScene} has failed");
        }
    }

    private IEnumerator FadeCurrentPage()
    {
        yield return StartCoroutine(fadeEffect.FadeOut());

        if (currentScene == activeSequenceLength-1)
        {
            StartCoroutine(ExitSequence());
        }
        else
        {
            progressButton.interactable = true;
        }
    }

    private IEnumerator ExitSequence()
    {
        controls.SetActive(true);

        for (int i = 0; i < controlsFadeEffects.Length - 1; i++)
        {
            StartCoroutine(controlsFadeEffects[i].FadeIn());
        }

        yield return StartCoroutine(controlsFadeEffects[controlsFadeEffects.Length - 1].FadeIn());

        stats.ShowStats();
    }

    [UsedImplicitly]
    public void OnGameEndReached()
    {
        isGoodEnding = stats.CollectedFeathers >= 10;
        activeSequenceLength = isGoodEnding ? goodEndingScenes.Length : badEndingScenes.Length;
        progressButton.interactable = false;
        LoadFirstScene();
    }

    private void LoadFirstScene()
    {
        AssetReferenceSprite[] activeScene = isGoodEnding ? goodEndingScenes : badEndingScenes;
        activeScene[0].LoadAssetAsync().Completed += (pageHandle) =>
        {
            back.sprite = pageHandle.Result;
            progressButton.interactable = true;
        };
    }

    private void LoadCurrentScene()
    {
        if (isGoodEnding)
        {
            goodEndingScenes[currentScene].LoadAssetAsync().Completed += OnPageLoaded;
        }
        else
        {
            badEndingScenes[currentScene].LoadAssetAsync().Completed += OnPageLoaded;
        }
    }

    [UsedImplicitly]
    public void ProgressOutro()
    {
        if (++currentScene == activeSequenceLength) return;

        LoadCurrentScene();
        progressButton.interactable = false;
    }

    [UsedImplicitly]
    public void QuitGame()
    {
        IsQuitting = true;
        Application.Quit();
    }

    [UsedImplicitly]
    public void CloseOutro()
    {
        currentScene = 0;

        if (isGoodEnding)
        {
            foreach (var spriteRef in goodEndingScenes)
            {
                spriteRef.ReleaseAsset();
            }
        }
        else
        {
            foreach (var spriteRef in badEndingScenes)
            {
                spriteRef.ReleaseAsset();
            }
        }

        controls.SetActive(false);
    }
}
