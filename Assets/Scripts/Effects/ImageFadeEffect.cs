using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeEffect : MonoBehaviour, IRestartable
{
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Image targetImage;
    private Color imageColor, originalColor;

    private void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        imageColor = targetImage.color;
        originalColor = imageColor;

        RegisterWithHandler();
    }

    public IEnumerator FadeOut()
    {
        while (imageColor.a > 0f)
        {
            imageColor.a -= fadeSpeed * Time.deltaTime;
            targetImage.color = imageColor;
            yield return null;
        }
    }

    public void Restart() => targetImage.color = originalColor;

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);

    private void OnDestroy()
    {
        if(EndScreenControls.IsQuitting) return;

        GameRestartHandler.UnRegisterRestartable(this);
    }
}
