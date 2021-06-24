using System.Collections;
using JetBrains.Annotations;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class IntroProgressController : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private MusicPiece mainMenuMusicPiece;
    [SerializeField] private MusicPiece introMusicPiece;

    [Header("Intro")] 
    [SerializeField] private GameObject introHint;
    [SerializeField] private Animator bubbleSequence;
    [SerializeField] private AssetReferenceSprite[] introScenes;
    private int currentScene;
    private Image front, back;
    private ImageFadeEffect fadeEffect;
    private Button progressButton;

    [Header("Loading")]
    [SerializeField] private Image loadProgressImage;

    private void Awake()
    {
        introHint.SetActive(false);
        progressButton = GetComponent<Button>();
        progressButton.interactable = false;

        front = transform.GetChild(1).GetComponent<Image>();
        fadeEffect = front.GetComponent<ImageFadeEffect>();
        back = transform.GetChild(0).GetComponent<Image>();
        
        StartCoroutine(WaitForBankLoaded());
    }

    #region Loading

    private IEnumerator WaitForBankLoaded()
    {
        var handle = introScenes[currentScene].LoadAssetAsync();

        yield return new WaitUntil(() => RuntimeManager.HasBankLoaded("Master"));

        yield return handle;

        back.sprite = handle.Result;

        StartCoroutine(FadeOutLogo());
        introHint.SetActive(true);

        yield return new WaitForSeconds(0.125f);
        introMusicPiece.PlaySolo();
    }

    private void OnPageLoaded(AsyncOperationHandle<Sprite> pageHandle)
    {
        if (pageHandle.Status != AsyncOperationStatus.Failed)
        {
            front.gameObject.SetActive(true);
            front.sprite = back.sprite;
            back.sprite = pageHandle.Result;
            front.color = back.color;

            fadeEffect.ResetEffect();
            StartCoroutine(FadeCurrentPage());
        }
        else
        {
            Debug.LogError($"Loading scene {currentScene} has failed");
        }
    }

    #endregion

    #region Fading

    private IEnumerator FadeCurrentPage()
    {
        yield return StartCoroutine(fadeEffect.FadeOut());
        
        progressButton.interactable = true;
    }

    private IEnumerator FadeOutLogo()
    {
        var fade = loadProgressImage.GetComponent<ImageFadeEffect>();

        yield return StartCoroutine(fade.FadeOut());

        Destroy(loadProgressImage);

        StartCoroutine(FadeCurrentPage());
    }

    private IEnumerator FadeToMain()
    {
        GetComponent<Image>().raycastTarget = false;

        var fade = bubbleSequence.GetComponent<ImageFadeEffect>();
        mainMenuMusicPiece.PlaySolo();

        yield return StartCoroutine(fade.FadeOut());

        StartCoroutine(CleanUpIntro());
    }

    #endregion

    private IEnumerator PlayExitSequence()
    {
        front.enabled = false;
        back.enabled = false;
        transform.GetChild(2).gameObject.SetActive(false);

        bubbleSequence.gameObject.SetActive(true);
        bubbleSequence.SetTrigger("Start");

        yield return new WaitUntil(() => bubbleSequence.GetCurrentAnimatorStateInfo(0).IsTag("End"));

        StartCoroutine(FadeToMain());
    }

    private IEnumerator CleanUpIntro()
    {
        foreach (var sprite in introScenes)
        {
            if (sprite.Asset == null) continue;
            
            sprite.ReleaseAsset();
            yield return null;
        }

        Destroy(gameObject);
    }

    #region Buttons


    [UsedImplicitly]
    public void ProgressIntro()
    {
        if(currentScene == 0)
            Destroy(introHint);

        if (++currentScene < introScenes.Length)
        {
            Debug.Log(currentScene);
            introScenes[currentScene].LoadAssetAsync().Completed += OnPageLoaded;
        }
        else
        {
            StartCoroutine(PlayExitSequence());
        }

        progressButton.interactable = false;
    }

    [UsedImplicitly]
    public void SkipIntro() => StartCoroutine(PlayExitSequence());

    #endregion
}
