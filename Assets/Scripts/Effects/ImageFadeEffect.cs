using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeEffect : MonoBehaviour, IRestartable
{
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Image targetImage;
    [SerializeField] private bool setActiveOnRestart;
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

        gameObject.SetActive(false);
    }

    public void Restart()
    {
        targetImage.color = originalColor;
        imageColor = originalColor;
        if(setActiveOnRestart)
            gameObject.SetActive(true);
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);

    private void OnDestroy()
    {
        if(EndScreenControls.IsQuitting) return;

        GameRestartHandler.UnRegisterRestartable(this);
    }
}
