using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class TutorialProgressController : MonoBehaviour, IRestartable
{
    [SerializeField] private Animator introAnimator;
    [SerializeField] private GameObject sideCharacter;

    [UsedImplicitly]
    public void StartListeningForIntroEnd()
    {
        introAnimator.SetTrigger("SceneStart");
        StartCoroutine(WatchForEndOfClips());
    }

    private IEnumerator WatchForEndOfClips()
    {
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => introAnimator.GetCurrentAnimatorStateInfo(0).IsTag("End"));

        gameObject.SetActive(false);
        sideCharacter.SetActive(true);
    }

    [UsedImplicitly]
    public void SkipIntro() => introAnimator.SetTrigger("SkipIntro");

    public void Restart() => introAnimator.SetTrigger("BackToMain");

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
