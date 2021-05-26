using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(ImageFadeEffect))]
public class ImageOnEventFader : AListenerEnabler
{
    private ImageFadeEffect fade;

    private void Awake() => fade = GetComponent<ImageFadeEffect>();

    [UsedImplicitly]
    public void OnGameStartFade() => StartCoroutine(fade.FadeOut());
}
