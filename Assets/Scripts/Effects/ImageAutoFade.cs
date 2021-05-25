using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(ImageFadeEffect))]
public class ImageAutoFade : AListenerEnabler
{
    private ImageFadeEffect fade;

    private void Awake() => fade = GetComponent<ImageFadeEffect>();

    [UsedImplicitly]
    public void OnGameStartFade() => StartCoroutine(AutoFade());

    private IEnumerator AutoFade()
    {
        yield return StartCoroutine(fade.FadeOut());

        transform.parent.gameObject.SetActive(false);
    }
}
